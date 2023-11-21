using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using TaskManager.API.ViewModels.Task;
using TaskManager.Core.Models.Task;
using TaskManager.Core.Services.TaskService;
using TaskManager.Core.Services.UserService;

namespace TaskManager.API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/v1/tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        public ITaskService _taskService { get; set; }
        public IUserService _userService { get; set; }
        public IMapper _mapper { get; set; }

        public TaskController(ITaskService taskService, IMapper mapper, IUserService userService)
        {
            _taskService = taskService;
            _mapper = mapper;
            _userService = userService;
        }

        class Temp
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Text { get; set; }
            public string Author { get; set; }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserIdFromRequest(HttpContext);

            var taskModels = await _taskService.GetTasksForUser(userId);

            return Ok(_mapper.Map<IEnumerable<TaskViewModel>>(taskModels));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskCreateRequestModel task)
        {
            var userId = GetUserIdFromRequest(HttpContext);

            var taskModel = _mapper.Map<TaskModel>(task);

            taskModel = await _taskService.Add(taskModel, userId);

            return Ok(_mapper.Map<TaskViewModel>(taskModel));
        }

        private int GetUserIdFromRequest(HttpContext context)
        {
            var token = GetJwtTokenFromRequest(context);
            var userId = GetIdFromToken(token);

            return int.Parse(userId);
        }

        private string GetJwtTokenFromRequest(HttpContext context)
        {
            var authHeader = HttpContext.Request.Headers["Authorization"];
            var token = authHeader.ToString().Replace("Bearer ", "");

            return token;
        }

        private string GetIdFromToken(string jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadToken(jwtToken) as JwtSecurityToken;
            var id = token.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value;
            return id;
        }
    }
}
