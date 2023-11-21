using TaskManager.API.Middlewares;

namespace TaskManager.API.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandling(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomeExceptionHandlingMiddleware>();

            return app;
        }
    }
}
