using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Core.Platform;

namespace SquirrelsNest.Core.Preferences {
    public interface IPreferencesHandler {
        T       Load<T>() where T : new();
        void    Save<T>( T preferences );
    }

    public class PreferencesHandler : IPreferencesHandler {
        private readonly IEnvironment   mEnvironment;
        private readonly IFileWriter    mFileWriter;
        private readonly ILog           mLog;

        public PreferencesHandler( IEnvironment environment, IFileWriter fileWriter, ILog log ) {
            mEnvironment = environment;
            mLog = log;
            mFileWriter = fileWriter;

        }

        private string FileNameForType<T>() => $"{typeof(T).Name}.json";

        private string GetFilePath( string fileName ) => Path.Combine( mEnvironment.PreferencesDirectory(), fileName );

        public T Load<T>() where T : new() {
            var retValue = new T();
            var fileName = FileNameForType<T>();

            try {
                var path = GetFilePath( fileName );

                mFileWriter.Load<T>( path )
                    .Do( result => retValue = result );
            }
            catch( Exception ex ) {
                mLog.LogException( $"Loading preferences from {fileName}", ex );
            }

            return retValue;
        }

        public void Save<T>( T settings ) {
            var fileName = FileNameForType<T>();

            try {
                mFileWriter.Save( GetFilePath( fileName ), settings );
            }
            catch( Exception ex ) {
                mLog.LogException( $"Saving preferences from {fileName}", ex );
            }
        }
    }
}
