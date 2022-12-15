using System;
using System.Diagnostics;

namespace SquirrelsNest.Pecan.Shared.Entities {
    [DebuggerDisplay("Type: {" + nameof( Name ) + "}")]
    public class SnIssueType :EntityBase {
        public  EntityId        ProjectId { get; }
        public  string          Name { get; }
        public  string          Description { get; }

        public SnIssueType( string entityId, string projectId, string name, string description ) :
            base( entityId ) {
            ProjectId = EntityId.CreateIdOrThrow( projectId );
            Name = name;
            Description = description;
        }

        public SnIssueType( string name ) :
            base( String.Empty ) {
            if( String.IsNullOrWhiteSpace( name )) throw new ApplicationException( "IssueType names cannot be empty" );

            ProjectId = EntityId.Default;
            Name = name;
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
            mDefault ??= new SnIssueType( EntityId.Default, EntityId.Default, "Unspecified", String.Empty );
    }
}
