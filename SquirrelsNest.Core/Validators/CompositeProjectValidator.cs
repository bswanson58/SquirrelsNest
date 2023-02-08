using FluentValidation;
using SquirrelsNest.Core.CompositeBuilders;

namespace SquirrelsNest.Core.Validators {
    // insures that a project is ready for use.
    internal class CompositeProjectValidator : AbstractValidator<CompositeProject> {
        public CompositeProjectValidator() {
            RuleFor( project => project.Project.Name ).NotEmpty();
            RuleFor( project => project.IssueTypes ).NotEmpty();
            RuleFor( project => project.WorkflowStates ).NotEmpty();
            RuleFor( project => project.Users ).NotEmpty();
        }
    }
}
