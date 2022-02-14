using LanguageExt.Common;
using LiteDB;
using SquirrelsNest.Common.Values;
using SquirrelsNest.LiteDb.Database;
using SquirrelsNest.LiteDb.Dto;
using Unit = LanguageExt.Unit;

namespace SquirrelsNest.LiteDb.Providers {
    internal class BaseProvider<TEntity, TDb> : EntityProvider<TDb> where TDb : DbBase {
        private readonly Func<TEntity, TDb>    mConvertToDbEntity;
        private readonly Func<TDb, TEntity>    mConvertFromDbEntity;    

        public BaseProvider( IDatabaseProvider databaseProvider, string collectionName, 
                             Func<TEntity, TDb> convertToDbEntity, Func<TDb, TEntity> convertFromDbEntity )
            : base( databaseProvider, collectionName ) {
            mConvertToDbEntity = convertToDbEntity;
            mConvertFromDbEntity = convertFromDbEntity;
        }

        public LanguageExt.Either<Error, TEntity> Add( TEntity entity ) {
            return InsertEntity( mConvertToDbEntity( entity ))
                .Map( dbEntity => mConvertFromDbEntity( dbEntity ));
        }

        public LanguageExt.Either<Error, Unit> Update( TEntity entity ) {
            return UpdateEntity( mConvertToDbEntity( entity ));
        }

        public LanguageExt.Either<Error, Unit> Delete( TEntity entity ) {
            return DeleteEntity( mConvertToDbEntity( entity ));
        }

        public LanguageExt.Either<Error, TEntity> Get( EntityId entityId, string dbIdName ) {
            return ValidateString( entityId )
                .Bind( _ => FindEntity( Query.EQ( dbIdName, entityId.Value )))
                .Map( dbEntity => mConvertFromDbEntity( dbEntity ));
        }

        public LanguageExt.Either<Error, IEnumerable<TEntity>> GetEnumerable() {
            return GetList()
                .Map( dbList => dbList.ToEnumerable())
                .Map( entityList => from entity in entityList select mConvertFromDbEntity( entity ));
        }
    }
}
