using System;
using Serilog;
using SquirrelsNest.Common.Interfaces;

namespace SquirrelsNest.Service.Support {
    public class WebPlatformLog : IApplicationLog {
        private readonly IEnvironment   mEnvironment;
        private readonly ILogger	mLog;

        public WebPlatformLog( IEnvironment environment ) {
            mEnvironment = environment;

            mLog = Log.Logger;
        }

        public void LogException( string message, Exception ex ) {
            mLog.Error( ex, message );
        }

        public void LogMessage( string message ) {
            mLog.Information( message );
        }
        public void ApplicationStarting() {
            mLog.Information( $"+++++ {mEnvironment.ApplicationName()} application starting +++++" );
        }

        public void ApplicationExiting() {
            mLog.Information( $"===== {mEnvironment.ApplicationName()} application exiting ======" );
        }
    }
}
