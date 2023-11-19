using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDDemo.Filters.ExceptionFilters
{
    public class HandlerExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<HandlerExceptionFilter> _logger;
        private readonly IHostEnvironment _hostEnvironment;

        public HandlerExceptionFilter(ILogger<HandlerExceptionFilter> logger, IHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _hostEnvironment = hostEnvironment;
                
        }
        public void OnException(ExceptionContext context)
        {
            _logger.LogError("Exception filter {FilterName}.{MethodName}\n{ExceptionType}\n{ExceptionMessage}", nameof(HandlerExceptionFilter), nameof(OnException), 
                context.Exception.GetType().ToString(), context.Exception.Message);

            if (_hostEnvironment.IsDevelopment())
            {
                context.Result = new ContentResult() { Content = context.Exception.Message, StatusCode = 500 };

            }
        }
    }
}
