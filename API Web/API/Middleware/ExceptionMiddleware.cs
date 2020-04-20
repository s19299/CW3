using System;
using System.Resources;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeTask(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                await handleException(context, e);
            }
           
        }

        private Task handleException(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            return context.Response.WriteAsync(new ErrorDetails{statusCode = StatusCodes.Status500InternalServerError, 
                message = "wystapil blad serwera" + exception}.ToString());
        }
    }
}