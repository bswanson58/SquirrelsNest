using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace SquirrelsNest.Service.Filters {
    public class ExceptionFilter : ExceptionFilterAttribute {
        private readonly ILogger<ExceptionFilter>   mLogger;

        public ExceptionFilter( ILogger<ExceptionFilter> logger ) {
            mLogger = logger;
        }

        public override void OnException( ExceptionContext context ) {
            mLogger.LogError( context.Exception, context.Exception.Message );

            base.OnException( context );
        }
    }
}
