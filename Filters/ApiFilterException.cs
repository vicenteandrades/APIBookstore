using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace APIBookstore.Filters
{
    public class ApiFilterException : IExceptionFilter
    {
        private readonly ILogger<ApiFilterException>  _logger;

        public ApiFilterException(ILogger<ApiFilterException> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, $"Aconteceu um erro - {DateTime.Now}");

            context.Result = new ObjectResult("Erro inesperado aconteceu...")
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

        }
    }
}
