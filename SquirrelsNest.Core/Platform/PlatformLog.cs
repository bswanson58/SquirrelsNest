using Serilog;
using SquirrelsNest.Common.Interfaces;

namespace SquirrelsNest.Core.Platform {
    internal class PlatformLog : IApplicationLog {
        private readonly IEnvironment   mEnvironment;
        private readonly ILogger	mLog;

        public PlatformLog( IEnvironment environment ) {
            mEnvironment = environment;

            var logFile = Path.Combine( environment.LogFileDirectory(), mEnvironment.ApplicationName() + " log - {Date}.log" );

            mLog = new LoggerConfiguration()
                .Enrich.WithProcessId()
                .WriteTo.RollingFile( logFile, outputTemplate:"{Timestamp:MM-dd-yy HH:mm:ss.ffff} [{ProcessId}] [{Level}] {Message}{NewLine}{Exception}",
                    fileSizeLimitBytes:8192 * 1024,	retainedFileCountLimit:10 )
#if DEBUG
                .WriteTo.Console()
#endif
                .CreateLogger();
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
