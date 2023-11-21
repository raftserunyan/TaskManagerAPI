using AutoMapper;
using TaskManager.Core.Models.Task;
using TaskManager.Core.Services.AuthorService;
using TaskManager.Core.Services.Common;
using TaskManager.Core.Services.UserService;
using TaskManager.Data.Entities;
using TaskManager.Data.Specifications.TaskSpecs;
using TaskManager.Data.UnitOfWork;

namespace TaskManager.Core.Services.TaskService
{
    internal class TaskService : CommonService<TaskModel, TaskEntity>, ITaskService
    {
        public IUserService _userService { get; set; }
        public IAuthorService _authorService { get; set; }

        public TaskService(IUnitOfWork uow, 
            IMapper mapper,
            IUserService userService,
            IAuthorService authorService) 
            : base(uow, mapper)
        {
            _userService = userService;
            _authorService = authorService;
        }

        public async Task<TaskModel> Add(TaskModel model, int userId)
        {
            var user = await _userService.GetUserById(userId.ToString());
            EnsureExists(user);

            var authorModel = await _authorService.GetAuthorByUser(user);

            if (authorModel.Id == default(int))
                model.Author = authorModel;
            else
                model.AuthorId = authorModel.Id;

            var taskEntity = _mapper.Map<TaskEntity>(model);

            await _uow.Repository<TaskEntity>().Add(taskEntity);
            await _uow.SaveChangesAsync();

            return _mapper.Map<TaskModel>(taskEntity);
        }

        public async Task<IEnumerable<TaskModel>> GetTasksForUser(int userId)
        {
            var user = await _userService.GetUserById(userId.ToString());
            var author = await _authorService.GetAuthorByUser(user);

            var taskEntities = await _uow.Repository<TaskEntity>()
                                    .GetAllBySpecification(new TasksByAuthorIdSpecification(author.Id.Value));

            var taskModels = _mapper.Map<IEnumerable<TaskModel>>(taskEntities);

            return taskModels;
        }
    }
}
