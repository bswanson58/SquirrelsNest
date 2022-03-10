using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces.Database;
using SquirrelsNest.Common.Values;
using SquirrelsNest.EfDb.Context;
using SquirrelsNest.EfDb.Dto;

namespace SquirrelsNest.EfDb.Providers {
    internal class ComponentProvider : EntityProvider<SnComponent, DbComponent>, IDbComponentProvider {
        public ComponentProvider( IContextFactory contextFactory )
            : base( contextFactory ) { }

        protected override SnComponent ConvertTo( DbComponent component ) => component.ToEntity();
        protected override DbComponent ConvertFrom( SnComponent component ) => DbComponent.From( component );

        public Task<Either<Error, SnComponent>> AddComponent( SnComponent component ) => AddEntity( component );
        public Task<Either<Error, Unit>> UpdateComponent( SnComponent component ) => UpdateEntity( component );
        public Task<Either<Error, Unit>> DeleteComponent( SnComponent component ) => DeleteEntity( component );
        public Task<Either<Error, SnComponent>> GetComponent( EntityId componentId ) => GetEntity( componentId );
        public Task<Either<Error, IEnumerable<SnComponent>>> GetComponents() => GetEntities();
        public Task<Either<Error, IEnumerable<SnComponent>>> GetComponents( SnProject forProject ) => 
            GetEntities( c => c.ProjectId.Equals( forProject.EntityId ));
    }
}
