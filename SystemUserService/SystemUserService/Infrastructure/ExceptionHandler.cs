using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace SystemUserService.Infrastructure
{
    public class ExceptionHandler : IExceptionFilter
    {
        private readonly ILogger<ExceptionHandler> _logger;
        public ExceptionHandler(ILogger<ExceptionHandler> logger)
        {
            _logger = logger;
        }
        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception.Message);
            _logger.LogTrace(context.Exception.StackTrace);
            context.Result = new ObjectResult("Server Error")
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }
}
