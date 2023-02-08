using LanguageExt.Common;
using LanguageExt;

namespace SquirrelsNest.Common.Interfaces.Database {
    public interface IDatabaseInitializer {
        Task<Either<Error, Unit>>   InitializeDatabase();
    }
}
