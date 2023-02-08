using System;
using System.Reflection;
using Microsoft.Toolkit.Mvvm.Input;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Desktop.Platform;

namespace SquirrelsNest.Desktop.ViewModels {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class StatusViewModel {
        private readonly IEnvironment   mEnvironment;
        private readonly ILog           mLog;

        public	string                  VersionString { get; }

        public  IRelayCommand           OpenDataFolder { get; }
        
        public StatusViewModel( IEnvironment environment, ILog log ) {
            mEnvironment = environment;
            mLog = log;

            var compileDate = ApplicationInformation.GetLinkerTime( Assembly.GetExecutingAssembly());
            var version = ApplicationInformation.GetVersion( Assembly.GetExecutingAssembly());

            var compileDateString = $"{compileDate.Month:D2}/{compileDate.Day:D2}/{compileDate.Year % 100:D2}";
            var versionString = version != null ? $"{version.Major}.{version.Minor}" : "unknown";

            VersionString = $"SquirrelsNest v{versionString} - {compileDateString}";

            OpenDataFolder = new RelayCommand( OnOpenDataFolder );
        }

        private void OnOpenDataFolder() {
            try {
                System.Diagnostics.Process.Start( 
                    new System.Diagnostics.ProcessStartInfo {
                            FileName = mEnvironment.ApplicationDirectory(),
                            UseShellExecute = true,
                            Verb = "open"
                        });
            }
            catch( Exception ex ) {
                mLog.LogException( "OnLaunchRequest:", ex );
            }
        }
    }
}
