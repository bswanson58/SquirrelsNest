﻿using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Core.Transfer.Import;

namespace SquirrelsNest.Core.Interfaces {
    public interface IImportManager {
        Task<Either<Error, SnProject>>  ImportProject( ImportParameters parameters, SnUser forUser );
        Task<Either<Error, SnProject>>  ImportProject( Stream input, ImportParameters parameters, SnUser forUser );
    }
}
