using System.Runtime.Serialization.Json;
using System.Net;
using System;
using System.Threading.Tasks;
using Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace API.Middleware {
    public class ErrorHandlingMiddleware {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        public ErrorHandlingMiddleware (RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger) 
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                await HandleExceptionAsync(context, ex, _logger);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex, ILogger<ErrorHandlingMiddleware> logger)
        {
            object errors = null;
            switch(ex)
            {
                case RestException re:
                _logger.LogError(ex, "REST ERROR");
                errors = re.Errors;
                context.Response.StatusCode = (int) re.StatusCode;
                break;

                case Exception e:
                _logger.LogError(ex, "SERVER ERROR");
                errors = string.IsNullOrWhiteSpace(e.Message)? "Error" : e.Message;
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
            }

            context.Response.ContentType = "application/json";
            if(errors!=null)
            {
                var result = JsonSerializer.Serialize(new {
                    errors
                });

                await context.Response.WriteAsync(result);
            }
        }
    }
}