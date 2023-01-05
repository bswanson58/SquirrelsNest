using System;
using System.Text.Json.Serialization;
using SquirrelsNest.Pecan.Shared.Platform;

namespace SquirrelsNest.Pecan.Shared.Entities {
    public class SnCompositeIssue {
        public  string              EntityId { get; }
        public  string              Title { get; }
        public  string              Description { get; }
        public  string              ProjectId { get; }
        public  uint                IssueNumber {  get; }
        public  DateOnly            EntryDate { get; }
        public  SnUser              EnteredBy { get; }
        public  SnIssueType         IssueType {  get; }
        public  SnComponent         Component { get; } 
        public  SnRelease           Release { get; }
        public  SnWorkflowState     WorkflowState { get; }
        public  SnUser              AssignedTo { get; }

        [JsonConstructor]
        public SnCompositeIssue( string entityId, string title, string description, string projectId, uint issueNumber, 
                                 DateOnly entryDate, SnUser enteredBy, SnIssueType issueType, SnComponent component,
                                 SnRelease release, SnWorkflowState workflowState, SnUser assignedTo ) {
            EntityId = entityId;
            Title = title;
            Description = description;
            ProjectId = projectId;
            IssueNumber = issueNumber;
            EntryDate = entryDate;
            EnteredBy = enteredBy;
            IssueType = issueType;
            Component = component;
            Release = release;
            WorkflowState = workflowState;
            AssignedTo = assignedTo;
        }

        public SnCompositeIssue( SnIssue fromIssue, SnUser enteredBy, SnIssueType issueType, SnComponent component,
                                 SnWorkflowState workflowState, SnRelease release, SnUser assignedTo ) {
            EntityId = fromIssue.EntityId;
            Title = fromIssue.Title;
            Description = fromIssue.Description;
            ProjectId = fromIssue.ProjectId;
            IssueNumber = fromIssue.IssueNumber;
            EntryDate = fromIssue.EntryDate;
            EnteredBy = enteredBy;
            IssueType = issueType;
            Component = component;
            Release = release;
            WorkflowState = workflowState;
            AssignedTo = assignedTo;
        }

        public SnCompositeIssue With( SnComponent component ) =>
            new ( EntityId, Title, Description, ProjectId, IssueNumber, EntryDate,
                  EnteredBy, IssueType, component, Release, WorkflowState, AssignedTo );

        public SnCompositeIssue With( SnIssueType issueType ) =>
            new ( EntityId, Title, Description, ProjectId, IssueNumber, EntryDate,
                EnteredBy, issueType, Component, Release, WorkflowState, AssignedTo );

        public SnCompositeIssue With( SnWorkflowState state ) =>
            new ( EntityId, Title, Description, ProjectId, IssueNumber, EntryDate,
                EnteredBy, IssueType, Component, Release, state, AssignedTo );

        private static SnCompositeIssue ? mDefaultIssue;

        public static SnCompositeIssue Default =>
            mDefaultIssue ??= new SnCompositeIssue( EntityIdentifier.Default, String.Empty, String.Empty, SnProject.Default.EntityId, 0, 
                                                    DateTimeProvider.Instance.CurrentDate, SnUser.Default, SnIssueType.Default, 
                                                    SnComponent.Default, SnRelease.Default, SnWorkflowState.Default, SnUser.Default );

    }
}
