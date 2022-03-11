using LanguageExt;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Core.CompositeBuilders;

namespace SquirrelsNest.Desktop.ViewModels.UiModels {
    internal class UiIssue : ObservableObject {
        private readonly CompositeIssue     mCompositeIssue;

        public  SnIssue             Issue => mCompositeIssue.Issue;
        public  SnIssueType         IssueType => mCompositeIssue.IssueType;
        public  SnComponent         Component => mCompositeIssue.Component;
        public  SnWorkflowState     State => mCompositeIssue.State;
        public  SnUser              AssignedUser => mCompositeIssue.AssignedTo;

        public  string              IssueNumber => $"{mCompositeIssue.Project.IssuePrefix}-{Issue.IssueNumber}";
        public  string              Title => Issue.Title;
        public  string              Description => Issue.Description;

        public  bool                IsFinalized => State.Category == StateCategory.Completed || State.Category == StateCategory.Terminal;
        public  bool                IsCurrentUser { get; private set; }

        public UiIssue( CompositeIssue compositeIssue, Option<SnUser> currentUser ) {
            mCompositeIssue = compositeIssue;

            currentUser.Do( u => IsCurrentUser = mCompositeIssue.Issue.AssignedToId.Equals( u.EntityId ));
        }
    }
}
