using System.Net;
using Avtobus1.Domain.CustomExceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Avtobus1.Infrastructure.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.ContentType = "application/json";
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    var exception = contextFeature.Error;

                    int statusCode;
                    string message;

                    switch (exception)
                    {
                        case NotFoundException notFoundException:
                            statusCode = (int)HttpStatusCode.NotFound;
                            message = notFoundException.Message;
                            break;
                        case AlreadyExistsException conflictException:
                            statusCode = (int)HttpStatusCode.Conflict;
                            message = conflictException.Message;
                            break;
                        case BadRequestException badRequestException:
                            statusCode = (int)HttpStatusCode.BadRequest;
                            message = badRequestException.Message;
                            break;
                        default:
                            statusCode = (int)HttpStatusCode.InternalServerError;
                            message = exception.Message;
                            break;
                    }

                    context.Response.StatusCode = statusCode;
                    await context.Response.WriteAsync(new ErrorDetails
                    {
                        StatusCode = statusCode,
                        Message = message
                    }.ToString());
                }
            });
        });
    }
}
