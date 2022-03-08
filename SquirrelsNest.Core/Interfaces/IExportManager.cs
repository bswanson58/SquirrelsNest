using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Core.Transfer.Export;

namespace SquirrelsNest.Core.Interfaces {
    public interface IExportManager {
        Task<Either<Error, Unit>>   ExportProject( ExportParameters parameters );
    }
}
