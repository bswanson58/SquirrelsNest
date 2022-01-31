using LanguageExt;
using LanguageExt.Common;
using LiteDB;
using SquirrelsNest.Common.Interfaces;

namespace SquirrelsNest.LiteDb.Database {
    internal class DatabaseProvider : IDatabaseProvider {
        private readonly IEnvironment           mEnvironment;
        private readonly IApplicationConstants  mApplicationConstants;
        private LiteDatabase ?                  mDatabase;

        public DatabaseProvider( IEnvironment environment, IApplicationConstants constants ) {
            mEnvironment = environment;
            mApplicationConstants = constants;
        }

        public Either<Error, LiteDatabase> GetDatabase() {
            if( mDatabase == null ) {
                try {
                    mDatabase = new LiteDatabase( DatabasePath());
                }
                catch ( Exception exception ) {
                    return Error.New( exception );
                }
            }

            return mDatabase;
        }

        private string DatabasePath() {
            return Path.Combine( mEnvironment.DatabaseDirectory(), mApplicationConstants.DatabaseFileName );
        }

        public void Dispose() {
            mDatabase?.Dispose();
            mDatabase = null;
        }
    }
}
