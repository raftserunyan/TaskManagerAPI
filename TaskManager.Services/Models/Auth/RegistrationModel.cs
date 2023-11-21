using System;

namespace TaskManager.Core.Models.Auth
{
    public class RegistrationModel
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
