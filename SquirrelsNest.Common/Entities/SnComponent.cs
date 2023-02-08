using System.Diagnostics;
using SquirrelsNest.Common.Values;

namespace SquirrelsNest.Common.Entities {
    [DebuggerDisplay("Component: {" + nameof( Name ) + "}")]
    public class SnComponent : EntityBase {
        public  EntityId    ProjectId { get; }
        public  string      Name { get; }
        public  string      Description { get; }

        public SnComponent( string entityId, string dbId, string projectId, string name, string description ) :
            base( entityId, dbId ) {
            ProjectId = EntityId.CreateIdOrThrow( projectId );
            Name = name;
            Description = description;
        }

        public SnComponent( string name ) :
            base( String.Empty ) {
            if( String.IsNullOrWhiteSpace( name )) throw new ApplicationException( "Component names cannot be empty" );

            ProjectId = EntityId.Default;
            Name = name;
            Description = String.Empty;
        }

        public SnComponent With( string ? name = null, string ? description = null ) {
            return new SnComponent( 
                EntityId, DbId, ProjectId,
                name ?? Name,
                description ?? Description );
        }

        public SnComponent For( SnProject project ) {
            if( project == null ) throw new ArgumentNullException( nameof( project ),  "Components cannot be set to a null project" );

            return new SnComponent( EntityId, DbId, project.EntityId, Name, Description );
        }

        private static SnComponent ? mDefaultComponent;

        public static SnComponent Default =>
            mDefaultComponent ??= new SnComponent( EntityId.Default, String.Empty, EntityId.Default, "Unspecified", String.Empty );
    }
}
