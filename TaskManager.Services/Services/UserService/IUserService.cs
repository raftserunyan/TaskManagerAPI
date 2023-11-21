using TaskManager.Data.Entities;

namespace TaskManager.Core.Services.UserService
{
    public interface IUserService
    {
        public Task<User> GetUserById(string userId);
    }
}
