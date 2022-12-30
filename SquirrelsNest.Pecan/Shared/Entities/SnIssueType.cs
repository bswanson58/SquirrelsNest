using System;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace SquirrelsNest.Pecan.Shared.Entities {
    [DebuggerDisplay("Type: {" + nameof( Name ) + "}")]
    public class SnIssueType :EntityBase, IComponentBase {
        public  string      ProjectId { get; }
        public  string      Name { get; }
        public  string      Description { get; }

        [JsonConstructor]
        public SnIssueType( string entityId, string projectId, string name, string description ) :
            base( entityId ) {
            ProjectId = EntityIdentifier.CreateIdOrThrow( projectId );
            Name = name;
            Description = description;
        }

        public SnIssueType( SnProject forProject ) {
            if( forProject == null ) throw new ArgumentNullException( nameof( forProject ),  "IssueTypes cannot be set to a null project" );

            ProjectId = forProject.EntityId;
            Name = String.Empty;
            Description = String.Empty;
        }

        public SnIssueType With( string ? name = null, string ? description = null ) {
            return new SnIssueType( 
                EntityId, ProjectId,
                name ?? Name,
                description ?? Description );
        }

        public SnIssueType For( SnProject project ) {
            if( project == null ) throw new ArgumentNullException( nameof( project ),  "IssueTypes cannot be set to a null project" );

            return new SnIssueType( EntityId, project.EntityId, Name, Description );
        }

        private static SnIssueType ? mDefault;

        public static SnIssueType Default => 
            mDefault ??= new SnIssueType( EntityIdentifier.Default, EntityIdentifier.Default, "Unspecified", String.Empty );
    }
}
