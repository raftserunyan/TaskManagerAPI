using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManager.Core.Models.Auth;
using TaskManager.Data.Entities;
using TaskManager.Shared.CustomExceptions;

namespace TaskManager.Core.Services.Auth
{
    public class AuthService : IAuthService
    {
        public UserManager<User> _userManager { get; set; }
        public IConfiguration _config { get; set; }
        private TokenValidationParameters _tokenValidationParameters;

        public AuthService(UserManager<User> userManager, IConfiguration config, TokenValidationParameters tokenValidationParameters)
        {
            _userManager = userManager;
            _config = config;
            _tokenValidationParameters = tokenValidationParameters;
        }

        // Public Methods
        public async Task Register(RegistrationModel registrationModel)
        {
            var user = new User()
            {
                FirstName = registrationModel.FirstName,
                LastName = registrationModel.SecondName,
                Email = registrationModel.Email,
                UserName = registrationModel.Email,
                BirthDate = registrationModel.BirthDate
            };

            var registrationResult = await _userManager.CreateAsync(user, registrationModel.Password);

            if (!registrationResult.Succeeded)
            {
                throw BadRequest(registrationResult.Errors.FirstOrDefault().Code + ": " + registrationResult.Errors.FirstOrDefault().Description);
            }
        }

        public async Task<AuthenticationResponseModel> SignIn(SignInRequestModel signInRequest)
        {
            var user = await _userManager.FindByEmailAsync(signInRequest.Username);

            if (user == null)
                throw new BadDataException($"Username '{signInRequest.Username}' is invalid");

            if (!await _userManager.CheckPasswordAsync(user, signInRequest.Password))
                throw GetInvalidCredentialsException();

            var authResponse = await CreateTokenAndRefreshToken(user);

            return authResponse;
        }

        public async Task<AuthenticationResponseModel> RefreshTokens(TokensSet tokensSet)
        {
            if (!TryGetPrincipalFromToken(tokensSet.AccessToken, out var principal))
            {
                throw new UnauthorizedAccessException();
            }

            var userId = principal.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            var userRefreshToken = await _userManager.GetAuthenticationTokenAsync(user, _config["TokenProviders:Default"], "refreshToken");
            if (String.IsNullOrWhiteSpace(userRefreshToken) || userRefreshToken != tokensSet.RefreshToken)
            {
                throw new UnauthorizedAccessException();
            }

            return await CreateTokenAndRefreshToken(user);
        }

        public async Task Logout(string accessToken)
        {
            if (!TryGetPrincipalFromToken(accessToken, out var principal))
            {
                throw new UnauthorizedAccessException();
            }

            var userId = principal.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            await _userManager.SetAuthenticationTokenAsync(user, _config["TokenProviders:Default"], "refreshToken", null);
        }

        // Private methods
        private async Task<AuthenticationResponseModel> CreateTokenAndRefreshToken(User user)
        {
            var accessToken = GenerateAccessToken(user);            
            var refreshToken = Guid.NewGuid().ToString();

            await _userManager.SetAuthenticationTokenAsync(user, _config["TokenProviders:Default"], "refreshToken", refreshToken);
          
            return new AuthenticationResponseModel
            {
                Token = accessToken,
                RefreshToken = refreshToken
            };
        }

        private string GenerateAccessToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim("UserId", user.Id.ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(double.Parse(_config["Jwt:AccessTokenLifetimeInMins"])),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private BadDataException BadRequest(string message)
        {
            throw new BadDataException(message);
        }

        private Exception GetInvalidCredentialsException()
        {
            return new BadDataException("Invalid Credentials");
        }

        private bool TryGetPrincipalFromToken(string token, out ClaimsPrincipal principal)
        {
            principal = null;
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var parsedToken = tokenHandler.ReadJwtToken(token);
                principal = new ClaimsPrincipal(new ClaimsIdentity(parsedToken.Claims)); //ValidateToken(token, _tokenValidationParameters, out var validatedToken);

                //bool isJwtTokenValid = (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                //   jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                //       StringComparison.InvariantCultureIgnoreCase);

                if (principal != null)
                {
                    return true;
                }
            }
            catch
            {
            }

            return false;
        }
    }
}
