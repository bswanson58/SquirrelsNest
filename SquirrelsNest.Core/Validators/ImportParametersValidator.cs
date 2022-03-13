using FluentValidation;
using SquirrelsNest.Core.Transfer.Import;

namespace SquirrelsNest.Core.Validators {
    internal class ImportParametersValidator : AbstractValidator<ImportParameters> {
        public ImportParametersValidator() {
            RuleFor( parameters => parameters.ProjectName ).NotEmpty();
            RuleFor( parameters => parameters.ImportFilePath ).NotEmpty();
            RuleFor( parameters => parameters.ImportFilePath ).Must( File.Exists ).WithMessage( "Input file does not exist." );
        }
    }
}
