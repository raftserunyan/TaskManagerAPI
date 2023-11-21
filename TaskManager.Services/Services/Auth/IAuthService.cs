using TaskManager.Core.Models.Auth;

namespace TaskManager.Core.Services.Auth
{
    public interface IAuthService
    {
        public Task Register(RegistrationModel registrationModel);
        public Task<AuthenticationResponseModel> SignIn(SignInRequestModel signInRequest);
        public Task<AuthenticationResponseModel> RefreshTokens(TokensSet tokensSet);
        public Task Logout(string accessToken);
    }
}
