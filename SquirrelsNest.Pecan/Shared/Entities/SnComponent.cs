using System;
using System.Diagnostics;

namespace SquirrelsNest.Pecan.Shared.Entities {
    [DebuggerDisplay("Component: {" + nameof( Name ) + "}")]
    public class SnComponent : EntityBase {
        public  EntityIdentifier    ProjectId { get; }
        public  string      Name { get; }
        public  string      Description { get; }

        public SnComponent( string entityId, string projectId, string name, string description ) :
            base( entityId ) {
            ProjectId = EntityIdentifier.CreateIdOrThrow( projectId );
            Name = name;
            Description = description;
        }

        public SnComponent( string name ) :
            base( String.Empty ) {
            if( String.IsNullOrWhiteSpace( name )) throw new ApplicationException( "Component names cannot be empty" );

            ProjectId = EntityIdentifier.Default;
            Name = name;
            Description = String.Empty;
        }

        public SnComponent With( string ? name = null, string ? description = null ) {
            return new SnComponent( 
                EntityId, ProjectId,
                name ?? Name,
                description ?? Description );
        }

        public SnComponent For( SnProject project ) {
            if( project == null ) throw new ArgumentNullException( nameof( project ),  "Components cannot be set to a null project" );

            return new SnComponent( EntityId, project.EntityId, Name, Description );
        }

        private static SnComponent ? mDefaultComponent;

        public static SnComponent Default =>
            mDefaultComponent ??= new SnComponent( EntityIdentifier.Default, EntityIdentifier.Default, "Unspecified", String.Empty );
    }
}
