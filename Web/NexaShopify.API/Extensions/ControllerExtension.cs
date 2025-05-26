using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace NexaShopify.API.Extensions
{
    public static class ControllerExtension
    {
        public static Core.Identity.Models.UserModel GetCurrentUser(this ControllerBase controller, bool includeAccess = true)
        {
            var user = new NexaShopify.Core.Identity.Handlers.User.GetHandler(controller.Request.Headers, includeAccess).Handle();
            Infrastructure.Services.Logging.Logger.LogFatal($"{user?.Id}|{parseRequest(controller)}"); // by adda isssa => So fatal because incoming request 
            return user;
        }
        static string parseRequest(ControllerBase controller) // return  string
        {
            return $"{controller?.HttpContext?.Request?.Path}|{(controller?.HttpContext?.Request?.Method)}|{controller?.HttpContext?.Request?.QueryString}";
        }

        public static IActionResult HandleException(this ControllerBase controller,
            Exception exception,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            if (exception is Core.Exceptions.UnauthorizedException)
            {
                Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn,
                    "Controller response: Not authorized",
                    memberName,
                    sourceFilePath,
                    sourceLineNumber);
                return controller.Ok(Core.Common.Models.ResponseModel<object>.AccessDeniedResponse()); //controller.Unauthorized();
            }
            else if (exception is Core.Exceptions.NotFoundException)
            {
                Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn,
                    "Controller response: Not found",
                    memberName,
                    sourceFilePath,
                    sourceLineNumber);
                return controller.Ok(Core.Common.Models.ResponseModel<object>.NotFoundResponse()); //controller.NotFound();
            }
            else
            {
                Infrastructure.Services.Logging.Logger.Log(exception.StackTrace); // - save stack trace
                Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn,
                    "Controller response: Error: " + exception.Message,
                    memberName,
                    sourceFilePath,
                    sourceLineNumber);
                return controller.Ok(Core.Common.Models.ResponseModel<object>.UnexpectedErrorResponse()); //controller.StatusCode(500);
            }
        }
        public static IActionResult HandleException(this ControllerBase controller,
            Exception exception,
            object input,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Infrastructure.Services.Logging.Logger.Log($"[Input] {System.Text.Json.JsonSerializer.Serialize(input)}");

            if (exception is Core.Exceptions.UnauthorizedException)
            {
                Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn,
                    "Controller response: Not authorized",
                    memberName,
                    sourceFilePath,
                    sourceLineNumber);
                return controller.Ok(Core.Common.Models.ResponseModel<object>.AccessDeniedResponse()); //controller.Unauthorized();
            }
            else if (exception is Core.Exceptions.NotFoundException)
            {
                Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn,
                    "Controller response: Not found",
                    memberName,
                    sourceFilePath,
                    sourceLineNumber);
                return controller.Ok(Core.Common.Models.ResponseModel<object>.NotFoundResponse()); //controller.NotFound();
            }
            else
            {
                Infrastructure.Services.Logging.Logger.Log(exception.StackTrace); // - save stack trace
                Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn,
                    "Controller response: Error: " + exception.Message,
                    memberName,
                    sourceFilePath,
                    sourceLineNumber);
                return controller.Ok(Core.Common.Models.ResponseModel<object>.UnexpectedErrorResponse()); //controller.StatusCode(500);
            }
        }

    }
}
