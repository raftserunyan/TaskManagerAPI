using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.ViewModels.Auth;
using TaskManager.Core.Models.Auth;
using TaskManager.Core.Services.Auth;

namespace TaskManager.API.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public IMapper _mapper { get; set; }
        public IAuthService _authService { get; set; }

        public AuthController(IMapper mapper, IAuthService authService)
        {
            _mapper = mapper;
            _authService = authService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest registrationRequest)
        {
            var registrationModel = _mapper.Map<RegistrationModel>(registrationRequest);

            await _authService.Register(registrationModel);

            return Ok();
        }

        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest signInRequest)
        {
            var signInRequestModel = _mapper.Map<SignInRequestModel>(signInRequest);

            var authResult = await _authService.SignIn(signInRequestModel);

            return Ok(authResult);
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> RefreshTokens([FromBody] TokensSet tokensSet)
        {
            var newTokens = await _authService.RefreshTokens(tokensSet);

            return Ok(newTokens);
        }

        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            var token = HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
            await _authService.Logout(token);

            return Ok();
        }
    }
}
