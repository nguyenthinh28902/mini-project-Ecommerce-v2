using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Project.Common.Exceptions;
using Project.Common.Models;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Localization;

namespace Project.Common.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        // Inject IStringLocalizer vào hàm InvokeAsync để tận dụng Culture hiện tại của Request
        public async Task InvokeAsync(HttpContext context, IStringLocalizer<SharedResource> localizer)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Một lỗi không mong đợi đã xảy ra: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex, localizer);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, IStringLocalizer<SharedResource> localizer)
        {
            var statusCode = (int)HttpStatusCode.InternalServerError;
            string messageKey = "Message_System_Error"; // Key mặc định nếu có lỗi không xác định

            switch (exception)
            {
                case UnauthorizedException authEx:
                    statusCode = authEx.StatusCode;
                    messageKey = authEx.Message; 
                    break;
                case ForbiddenException forbEx:
                    statusCode = forbEx.StatusCode;
                    messageKey = forbEx.Message;
                    break;
                case BadRequestException badRequestEx:
                    statusCode = badRequestEx.StatusCode;
                    messageKey = badRequestEx.Message;
                    break;
                case ConflictException conflictEx:
                    statusCode = conflictEx.StatusCode;
                    messageKey = conflictEx.Message;
                    break;
                case NotFoundException notFoundException:
                    statusCode = notFoundException.StatusCode;
                    messageKey = notFoundException.Message;
                    break;
            }

            var localizedMessage = localizer[messageKey];

            // KIỂM TRA XEM REQUEST NÀY LÀ  API HAY LÀ GIAO DIỆN MVC THƯỜNG
            bool isApiCall = context.Request.Headers["X-Requested-With"] == "XMLHttpRequest"
                              || context.Request.Headers["Accept"].ToString().Contains("application/json");

            if (!isApiCall)
            {
                if (statusCode == (int)HttpStatusCode.Forbidden)
                {
                    context.Response.Redirect("/Home/AccessDenied");
                    return;
                }
                if (statusCode == (int)HttpStatusCode.Unauthorized)
                {
                    context.Response.Redirect("/Auth/Login");
                    return;
                }
            }

            // Trả về JSON chuẩn kèm thông điệp đã được dịch
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new
            {
                isSuccess = false,
                statusCode = statusCode,
                error = localizedMessage, // Đã được dịch theo đúng ngôn ngữ của user
                detail = _env.IsDevelopment() ? exception.ToString() : null
            };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
    }
}