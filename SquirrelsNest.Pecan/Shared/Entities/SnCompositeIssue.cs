using System;
using System.Text.Json.Serialization;

namespace SquirrelsNest.Pecan.Shared.Entities {
    public class SnCompositeIssue {
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
        public SnCompositeIssue( string title, string description, string projectId, uint issueNumber, DateOnly entryDate,
                                 SnUser enteredBy, SnIssueType issueType, SnComponent component,
                                 SnRelease release, SnWorkflowState workflowState, SnUser assignedTo ) {
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
    }
}
