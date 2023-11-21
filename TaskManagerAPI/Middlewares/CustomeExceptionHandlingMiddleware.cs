using System.Dynamic;
using System.Net;
using System.Security.Authentication;
using System.Text.Json;
using TaskManager.Shared.CustomExceptions;

namespace TaskManager.API.Middlewares
{
    public class CustomeExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomeExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "text";


                switch (error)
                {
                    case EntityNotFoundException e:
                        {
                            response.StatusCode = (int)HttpStatusCode.NotFound;
                            break;
                        }
                    case BadDataException e:
                        {
                            response.StatusCode = (int)HttpStatusCode.BadRequest;
                            break;
                        }
                    case UnauthorizedAccessException e:
                        {
                            response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            break;
                        }
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                await response.WriteAsync(error.Message);
            }
        }
    }
}
