using System.Reactive.Linq;
using System.Reactive.Subjects;
using LanguageExt.Common;
using LiteDB;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Values;
using SquirrelsNest.LiteDb.Database;
using SquirrelsNest.LiteDb.Dto;
using Unit = LanguageExt.Unit;

namespace SquirrelsNest.LiteDb.Providers {
    internal class BaseProvider<TEntity, TDb> : EntityProvider<TDb> where TDb : DbBase {
        private readonly Subject<EntitySourceChange>    mChangeSubject;
        private readonly Func<TEntity, TDb>             mConvertToDbEntity;
        private readonly Func<TDb, TEntity>             mConvertFromDbEntity;

        public  IObservable<EntitySourceChange>         OnEntitySourceChange => mChangeSubject.AsObservable();

        protected BaseProvider( IDatabaseProvider databaseProvider, string collectionName, 
                             Func<TEntity, TDb> convertToDbEntity, Func<TDb, TEntity> convertFromDbEntity )
            : base( databaseProvider, collectionName ) {
            mConvertToDbEntity = convertToDbEntity;
            mConvertFromDbEntity = convertFromDbEntity;

            mChangeSubject = new Subject<EntitySourceChange>();
        }

        protected LanguageExt.Either<Error, TEntity> Add( TEntity entity ) {
            var retValue = InsertEntity( mConvertToDbEntity( entity ))
                .Map( dbEntity => mConvertFromDbEntity( dbEntity ));

            mChangeSubject.OnNext( EntitySourceChange.EntityInserted );

            return retValue;
        }

        protected LanguageExt.Either<Error, Unit> Update( TEntity entity ) {
            var retValue = UpdateEntity( mConvertToDbEntity( entity ));

            mChangeSubject.OnNext( EntitySourceChange.EntityUpdated );

            return retValue;
        }

        protected LanguageExt.Either<Error, Unit> Delete( TEntity entity ) {
            var retValue = DeleteEntity( mConvertToDbEntity( entity ));

            mChangeSubject.OnNext( EntitySourceChange.EntityDeleted );

            return retValue;
        }

        protected LanguageExt.Either<Error, TEntity> Get( EntityId entityId, string dbIdName ) {
            return ValidateString( entityId )
                .Bind( _ => FindEntity( Query.EQ( dbIdName, entityId.Value )))
                .Map( dbEntity => mConvertFromDbEntity( dbEntity ));
        }

        protected LanguageExt.Either<Error, IEnumerable<TEntity>> GetEnumerable() {
            return GetList()
                .Map( dbList => dbList.ToEnumerable())
                .Map( entityList => from entity in entityList select mConvertFromDbEntity( entity ));
        }
    }
}
