using Contracts;
using Entities.ErrorModel;
using Entities.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace CompanyEmployees.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILoggerManager logger)
    {
        using var scope = app.ApplicationServices.CreateScope();

        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.ContentType = "application/json";
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    switch (contextFeature.Error)
                    {
                        case NotFoundException:
                            context.Response.StatusCode = StatusCodes.Status404NotFound;
                            break;
                        case BadRequestException:
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            break;
                        default:
                            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                            break;
                    }
                    logger.LogError($"Something went wrong: {contextFeature.Error}");
                    await context.Response.WriteAsync(new ErrorDetails
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = $"{contextFeature.Error.Message}",
                    }.ToString());
                }
            });
        });
    }
}