using LanguageExt;
using LanguageExt.Common;
using LiteDB;

namespace SquirrelsNest.LiteDb.Database {
    internal interface IDatabaseProvider : IDisposable {
        Either<Error, LiteDatabase>    GetDatabase();
    }
}
