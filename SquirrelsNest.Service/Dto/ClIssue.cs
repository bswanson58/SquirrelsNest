using System;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Core.CompositeBuilders;

namespace SquirrelsNest.Service.Dto {
    public class ClIssue : ClBase {
        public  string          Title { get; }
        public  string          Description { get; }
        public  ClProject       Project { get; }
        public  int             IssueNumber {  get; }
        public  DateOnly        EntryDate { get; }
        public  ClUser          EnteredBy { get; }
        public  ClIssueType     IssueType {  get; }
        public  ClComponent     Component { get; } 
        public  ClRelease       Release { get; }
        public  ClWorkflowState WorkflowState { get; }
        public  ClUser          AssignedTo { get; }
        public  bool            IsFinalized => WorkflowState.Category is StateCategory.Completed or StateCategory.Terminal;

        public ClIssue( string id, string title, string description, ClProject project, int issueNumber, DateOnly entryDate,
                        ClUser enteredBy, ClIssueType issueType, ClComponent component, ClRelease release, ClWorkflowState workflowState, 
                        ClUser assignedTo ) :
            base( id ) {
            Title = title;
            Description = description;
            Project = project;
            IssueNumber = issueNumber;
            EntryDate = entryDate;
            EnteredBy = enteredBy;
            IssueType = issueType;
            Component = component;
            Release = release;
            WorkflowState = workflowState;
            AssignedTo = assignedTo;
        }
    }

    public static class IssueExtensions {
        public static ClIssue ToCl( SnIssue issue ) {
            return new ClIssue( issue.EntityId, issue.Title, issue.Description, ClProject.Default, (int)issue.IssueNumber, issue.EntryDate,
                       ClUser.Default, ClIssueType.Default, ClComponent.Default, ClRelease.Default, ClWorkflowState.Default, ClUser.Default );
        }

        public static ClIssue ToCl( CompositeIssue issue ) {
            return new ClIssue( issue.Issue.EntityId, issue.Issue.Title, issue.Issue.Description,
                                issue.Project.ToCl(), (int)issue.Issue.IssueNumber, issue.Issue.EntryDate,
                                issue.EnteredBy.ToCl(), issue.IssueType.ToCl(), issue.Component.ToCl(),
                                ClRelease.Default, issue.State.ToCl(), issue.AssignedTo.ToCl());
        }
    }

}
