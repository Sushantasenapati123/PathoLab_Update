using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Patholab.Shared.Models;

namespace Patholab.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred during request execution.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var message = "An unexpected error occurred while processing your request.";
            var errors = new List<string>();

            // Custom domain exception checking if required
            if (exception is ArgumentException || exception is InvalidOperationException || exception.Message.Contains("not found") || exception.Message.Contains("already exists") || exception.Message.Contains("invalid"))
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                message = exception.Message;
            }
            else
            {
                errors.Add(exception.Message); // Include generic message in errors array for developer debugging in demo
            }

            var response = ApiResponse.CreateError(message, errors);
            var json = JsonSerializer.Serialize(response);

            return context.Response.WriteAsync(json);
        }
    }
}
