using System;
using System.IO;
using SquirrelsNest.Common.Interfaces;

namespace SquirrelsNest.Desktop.Preferences {
    public interface IPreferencesManager<T> where T: new() {
        T       Load();
        void    Save( T preferences );
    }

    public class PreferencesManager<T> : IPreferencesManager<T> where T : PreferencesManager<T>, new() {
        private readonly IEnvironment   mEnvironment;
        private readonly IFileWriter<T> mFileWriter;
        private readonly ILog           mLog;
        private readonly string         mFileName;

        protected PreferencesManager( IEnvironment environment, IFileWriter<T> fileWriter, ILog log ) {
            mEnvironment = environment;
            mLog = log;
            mFileWriter = fileWriter;

            mFileName = $"{typeof(T).Name}.json";
        }

        private string GetFilePath( string fileName ) => Path.Combine( mEnvironment.PreferencesDirectory(), fileName );

        public T Load() {
            var retValue = new T();

            try {
                var path = GetFilePath( mFileName );

                retValue = mFileWriter.Load( path );
            }
            catch( Exception ex ) {
                mLog.LogException( $"Loading preferences from {mFileName}", ex );
            }

            return retValue;
        }

        public void Save( T settings ) {
            try {
                mFileWriter.Save( GetFilePath( mFileName ), settings );
            }
            catch( Exception ex ) {
                mLog.LogException( $"Saving preferences from {mFileName}", ex );
            }
        }
    }
}
