using SquirrelsNest.Common.Interfaces;

namespace SquirrelsNest.Core.Environment {
    internal class ApplicationEnvironment : IEnvironment {
        private readonly IApplicationConstants  mApplicationConstants;

        public ApplicationEnvironment( IApplicationConstants applicationConstants ) {
            mApplicationConstants = applicationConstants;
        }

        public string ApplicationName() {
            return mApplicationConstants.ApplicationName;
        }

        public string EnvironmentName() {
            return System.Environment.MachineName;
        }

        public string ApplicationDirectory() {
            var retValue = Path.Combine( System.Environment.GetFolderPath( System.Environment.SpecialFolder.CommonApplicationData ),
                mApplicationConstants.CompanyName,
                mApplicationConstants.ApplicationName );

            if(!Directory.Exists( retValue )) {
                Directory.CreateDirectory( retValue );
            }

            return( retValue );
        }

        public string DatabaseDirectory() {
            var retValue = Path.Combine( ApplicationDirectory(), mApplicationConstants.DatabaseDirectory );

            if(!Directory.Exists( retValue )) {
                Directory.CreateDirectory( retValue );
            }

            return( retValue );
        }

        public string LogFileDirectory() {
            var retValue = Path.Combine( ApplicationDirectory(), mApplicationConstants.LogFileDirectory );

            if(!Directory.Exists( retValue )) {
                Directory.CreateDirectory( retValue );
            }

            return( retValue );
        }

        public string PreferencesDirectory() {
            var retValue = Path.Combine( ApplicationDirectory(), mApplicationConstants.ConfigurationDirectory );

            if(!Directory.Exists( retValue )) {
                Directory.CreateDirectory( retValue );
            }

            return( retValue );
        }
    }
}
