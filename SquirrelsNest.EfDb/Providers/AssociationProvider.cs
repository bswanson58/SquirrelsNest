using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces.Database;
using SquirrelsNest.Common.Values;
using SquirrelsNest.EfDb.Context;
using SquirrelsNest.EfDb.Dto;

namespace SquirrelsNest.EfDb.Providers {
    internal class AssociationProvider : EntityProvider<SnAssociation, DbAssociation>, IDbAssociationProvider {
        public AssociationProvider( IContextFactory contextFactory )
            : base( contextFactory ) { }

        protected override SnAssociation ConvertTo( DbAssociation user ) => user.ToEntity();
        protected override DbAssociation ConvertFrom( SnAssociation user ) => DbAssociation.From( user );

        public Task<Either<Error, SnAssociation>> AddAssociation( SnAssociation user ) => AddEntity( user );
        public Task<Either<Error, Unit>> DeleteAssociation( SnAssociation user ) => DeleteEntity( user );
        public Task<Either<Error, SnAssociation>> GetAssociation( EntityId userId ) => GetEntity( userId );

        public Task<Either<Error, IEnumerable<SnAssociation>>> GetAssociations( SnUser forUser ) => 
            GetEntities( a => a.OwnerId.Equals( forUser.EntityId ));
        public Task<Either<Error, IEnumerable<SnAssociation>>> GetAssociations( EntityId associationId ) =>
            GetEntities( a => a.AssociationId.Equals( associationId ));
    }
}
