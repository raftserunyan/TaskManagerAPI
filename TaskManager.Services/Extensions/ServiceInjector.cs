using Microsoft.Extensions.DependencyInjection;
using TaskManager.Core.Services.Auth;
using TaskManager.Core.Services.AuthorService;
using TaskManager.Core.Services.TaskService;
using TaskManager.Core.Services.UserService;
using TaskManager.Data.UnitOfWork;

namespace TaskManager.Core.Extensions
{
    public static class ServiceInjector
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthorService, AuthorService>();

            return services;
        }
    }
}
