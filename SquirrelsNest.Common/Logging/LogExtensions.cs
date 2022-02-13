using LanguageExt.Common;
using SquirrelsNest.Common.Interfaces;

namespace SquirrelsNest.Common.Logging {
    public static class LogExtensions {
        public static void LogError( this ILog log, Error error ) {
            if( error.Exception.IsSome ) {
                error.Exception.Do( ex => log.LogException( error.Message, ex ));
            }
            else {
                log.LogException( error.Message, new ApplicationException( "Unused exception" ));
            }
        }
    }
}
