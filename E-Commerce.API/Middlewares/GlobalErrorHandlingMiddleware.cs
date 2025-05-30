
using Domain.Exceptions;
using Shared.ErrorModels;
using System.Net;

namespace E_Commerce.API.Middlewares
{

    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

        public GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);

                if (httpContext.Response.StatusCode == (int)HttpStatusCode.NotFound)
                    await HandleNotFoundEndPointAsync(httpContext);

            }
            catch (Exception exception)
            {
                _logger.LogError($"something went wrong {exception}");


                await HandelExceptionAsync(httpContext, exception);
            }
        }

        private async Task HandleNotFoundEndPointAsync(HttpContext httpContext)
        {
            httpContext.Response.ContentType = "application/json";
            var response = new ErrorDetails
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                ErrorMessage = $"The End Point {httpContext.Request.Path} Not Found"
            }.ToString();

            await httpContext.Response.WriteAsync(response);
        }

        private async Task HandelExceptionAsync(HttpContext httpContext, Exception exception)
        {

            // set default status code to 500
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            // set content type => Application\json
            httpContext.Response.ContentType = "application/json";

            var response = new ErrorDetails
            {
                ErrorMessage = exception.Message
            };
            // C# 8.0 Feature
            httpContext.Response.StatusCode = exception switch
            {
                NotFoundException => (int)HttpStatusCode.NotFound,
                UnAuthorizedException => (int)HttpStatusCode.Unauthorized,
                ValidationException validationException => HandleValidationException(validationException, response),
                _ => (int)HttpStatusCode.InternalServerError
            };
            // return standard response 
            response.StatusCode = httpContext.Response.StatusCode;


            await httpContext.Response.WriteAsync(response.ToString());
        }

        private int HandleValidationException(ValidationException validationException, ErrorDetails response)
        {
            response.Errors = validationException.Errors;
            return (int)HttpStatusCode.BadRequest;
        }
    }
}
