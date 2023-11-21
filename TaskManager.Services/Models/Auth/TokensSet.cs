using System;

namespace TaskManager.Core.Models.Auth
{
    public class TokensSet
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
