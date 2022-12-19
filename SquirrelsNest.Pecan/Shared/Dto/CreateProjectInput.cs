using System;
using FluentValidation;

namespace SquirrelsNest.Pecan.Shared.Dto {
    public class CreateProjectInput {
        public  string      Name { get; set; }
        public  string      Description { get; set; }
        public  string      IssuePrefix { get; set; }

        public CreateProjectInput() {
            Name = String.Empty;
            Description = String.Empty;
            IssuePrefix = String.Empty;
        }
    }

    public class CreateProjectInputValidator : AbstractValidator<CreateProjectInput> {
        public CreateProjectInputValidator() {
            RuleFor( p => p.Name )
                .NotEmpty()
                .WithMessage( "A name for the project is required." );

            RuleFor( p => p.Description )
                .NotNull()
                .WithMessage( "The project description may not be null." );

            RuleFor( p => p.IssuePrefix )
                .NotEmpty()
                .WithMessage( "An issue prefix is required." );
        }
    }
}
