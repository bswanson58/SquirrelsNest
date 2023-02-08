using System;
using System.Collections.Generic;
using System.Linq;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Values;
using SquirrelsNest.Core.CompositeBuilders;

namespace SquirrelsNest.Service.Dto {
    public class ClProjectBase : ClBase {
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public string   Name { get; }
        public string   Description { get; }
        public DateOnly Inception { get; }
        public string   RepositoryUrl { get; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global

        public ClProjectBase( string id, string name, string description, DateOnly inception, string repositoryUrl ) :
            base( id ) {
            Name = name;
            Description = description;
            Inception = inception;
            RepositoryUrl = repositoryUrl;
        }

        private static ClProjectBase ? mDefaultProject;

        public static ClProjectBase Default =>
            mDefaultProject ??= new ClProjectBase( EntityId.Default.Value, "Unspecified", String.Empty, DateOnly.MinValue, String.Empty );
    }

    public class ClProject : ClProjectBase {
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public string                   IssuePrefix { get; }
        public int                      NextIssueNumber { get; }
        public List<ClComponent>        Components { get; }
        public List<ClIssueType>        IssueTypes { get; }
        public List<ClWorkflowState>    WorkflowStates { get; }
        public List<ClUser>             Users { get; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global

        public ClProject( string id, string name, string description, DateOnly inception, string repositoryUrl,
                                    string issuePrefix, int nextIssueNumber,
                                    IEnumerable<SnComponent> components, IEnumerable<SnIssueType> issueTypes,
                                    IEnumerable<SnWorkflowState> states, IEnumerable<SnUser> users ) :
            base( id, name, description, inception, repositoryUrl ) {
            IssuePrefix = issuePrefix;
            NextIssueNumber = nextIssueNumber;
            Components = new List<ClComponent>( from c in components select c.ToCl());
            IssueTypes = new List<ClIssueType>( from i in issueTypes select i.ToCl());
            WorkflowStates = new List<ClWorkflowState>( from s in states select s.ToCl());
            Users = new List<ClUser>( from u in users select u.ToCl());
        }

    }

    public static class ProjectExtensions {
        public static ClProjectBase ToCl( this SnProject project ) {
            return new ClProjectBase( project.EntityId, project.Name, project.Description, project.Inception,
                project.RepositoryUrl );
        }

        public static ClProject ToCl( this CompositeProject project ) {
            return new ClProject( project.Project.EntityId, project.Project.Name, project.Project.Description, 
                                            project.Project.Inception, project.Project.RepositoryUrl, project.Project.IssuePrefix, 
                                            (int)project.Project.NextIssueNumber, project.Components, project.IssueTypes,
                                            project.WorkflowStates, project.Users );
        }
    }
}
