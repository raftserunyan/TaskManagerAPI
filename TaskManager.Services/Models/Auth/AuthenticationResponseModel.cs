using System;

namespace TaskManager.Core.Models.Auth
{
    public class AuthenticationResponseModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
