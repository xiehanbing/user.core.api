using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace User.Core.Api.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment _environment;
        private readonly ILogger _logger;
        public GlobalExceptionFilter(IHostingEnvironment environment, ILogger<GlobalExceptionFilter> logger)
        {
            _environment = environment;
            _logger = logger;
        }
        public void OnException(ExceptionContext context)
        {
            var json = new JsonErrorResponse();
            if (context.Exception.GetType() == typeof(UserOperationException))
            {
                json.Message = context.Exception.Message;
                _logger.LogError(context.Exception.Message);
                context.Result = new BadRequestObjectResult(json);
            }
            else
            {
                json.Message = "发生了未知的内部错误";
                if (_environment.IsDevelopment())
                {
                    json.DevelopMessage =context.Exception.Message+"\n"+ context.Exception.StackTrace;
                }
               
                context.Result = new InternalServerErrorObjectResult(json);
            }
            _logger.LogError(context.Exception,context.Exception.Message);
            context.ExceptionHandled = true;
        }
    }

    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object error) : base(error)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}