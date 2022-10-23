using HealthTrackingSystem.Application.Exceptions;

namespace HealthTrackingSystem.API.Middlewares {
    public class ExceptionHandlingMiddleware {

        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context) {
            try
            {
                await _next(context);
            }
            catch (EntityNotFoundException exception)
            {
                // logger.LogError(exception, "An exception was thrown as a result of the request");
                context.Response.StatusCode = StatusCodes.Status404NotFound;
            }
            catch (Exception exception) {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync(exception.ToString());
                // HandleException(context, exception, logger);
            }
        }

        // private void HandleException(HttpContext context, Exception exception, ILogger logger) {
        //
        //     logger.LogError(exception, "An exception was thrown as a result of the request");
        //     context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        // }
    }
}
