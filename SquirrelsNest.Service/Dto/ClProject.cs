using System;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Values;

namespace SquirrelsNest.Service.Dto {
    public class ClProject : ClBase {
        public string Name { get; }
        public string Description { get; }
        public DateOnly Inception { get; }
        public string RepositoryUrl { get; }
        public string IssuePrefix { get; }
        public int NextIssueNumber { get; }

        public ClProject( string id, string name, string description, DateOnly inception, string repositoryUrl,
                          string issuePrefix, int nextIssueNumber ) :
            base( id ) {
            Name = name;
            Description = description;
            Inception = inception;
            RepositoryUrl = repositoryUrl;
            IssuePrefix = issuePrefix;
            NextIssueNumber = nextIssueNumber;
        }

        private static ClProject ? mDefaultProject;

        public static ClProject Default =>
            mDefaultProject ??= new ClProject( EntityId.Default.Value, "Unspecified", String.Empty, DateOnly.MinValue, String.Empty, String.Empty, 0 );
    }

    public static class ProjectExtensions {
        public static ClProject From( this SnProject project ) {
            return new ClProject( project.EntityId, project.Name, project.Description, project.Inception,
                                  project.RepositoryUrl, project.IssuePrefix, (int)project.NextIssueNumber );
        }
    }
}
