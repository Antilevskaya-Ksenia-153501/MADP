using Serilog.Core;

namespace WEB_153501_Antilevskaya.Middleware
{
    public class LoggingMiddleware
    {
        readonly RequestDelegate _next;
        readonly Logger _logger;

        public LoggingMiddleware(RequestDelegate next, Logger logger)
        {
            _next= next;
            _logger= logger;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next.Invoke(context);
            if (context.Response.StatusCode < 200 || context.Response.StatusCode >= 300)
            {
                var logMessage = $"---> request {context.Request.Path} returns {context.Response.StatusCode}";
                _logger.Information(logMessage);
            }
        }
    }
}
