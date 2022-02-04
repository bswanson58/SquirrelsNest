using LanguageExt;
using LanguageExt.Common;
using LiteDB;
using SquirrelsNest.Common.Exceptions;
using SquirrelsNest.LiteDb.Dto;

namespace SquirrelsNest.LiteDb.Database {
    internal class EntityProvider<T> : ValidationBase<T>, IDisposable where T : DbBase {
        private readonly IDatabaseProvider  mDatabaseProvider;
        private readonly string             mCollectionName;
        private LiteDatabase ?              mDatabase;

        internal EntityProvider( IDatabaseProvider databaseProvider, string collectionName ) {
            mDatabaseProvider = databaseProvider;
            mCollectionName = collectionName;
        }

        protected Either<Error, LiteDatabase> CreateConnection() {
            if( mDatabase == null ) {
                return mDatabaseProvider.GetDatabase()
                    .Map( db => {
                        mDatabase = db;

                        InitializeDatabase( mDatabase );
                        
                        return mDatabase;
                    });
            }

            return mDatabase;
        }

        protected virtual void InitializeDatabase( LiteDatabase db ) { }
        protected virtual ILiteCollection<T> Include( ILiteCollection<T> list ) {
            return list;
        }

        private Try<Unit> ScanCollection( LiteDatabase db, Action<ILiteCollection<T>> withAction ) {
            return Prelude.Try( () => {
                withAction( Include( db.GetCollection<T>( mCollectionName )));

                return Unit.Default;
            });
        }

        protected Either<Error, Unit> WithCollection( Action<ILiteCollection<T>> action ) {
            return
                from va in ValidateAction( action )
                    from db in CreateConnection()
                        from ret in ScanCollection( db, va ).ToEither( Error.New )
                            select ret;
        }

        private Either<Error, T> FindWithExpression( string expression ) {
            try {
                T ? retValue = default;

                WithCollection( collection => {
                    retValue = collection.FindOne( expression );
                });

                if( retValue == null ) {
                    return Error.New( "Item could not be located." );
                }

                return retValue;
            }
            catch( Exception ex ) {
                return Error.New( ex );
            }
        }

        protected Either<Error, T> FindEntity( string expression ) {
            return FindWithExpression( expression );
        }

        /*
        private Try<Unit> FindListWithExpression( string expression, Action<IEnumerable<T>> action ) {
            return Prelude.Try( () => {
                WithCollection( c => action( c.Find( expression )));

                return Unit.Default;
            });
        }

        protected Either<Error, Unit> FindEntityList( string expression, Action<IEnumerable<T>> action ) {
            return ValidateAction( action )
                .Bind( _ => FindListWithExpression( expression, action ).ToEither( Error.New ));
        }
        */
        /*
        private Try<Unit> QueryEntities( LiteDatabase db, Action<ILiteQueryable<T>> queryAction ) {
            return Prelude.Try( () => {
                queryAction( Include( db.GetCollection<T>( mCollectionName )).Query());

                return Unit.Default;
            });
        }

        protected Either<Error, Unit> QueryEntities( Action<ILiteQueryable<T>> action ) {
            return ValidateAction( action )
                .Bind( _ => CreateConnection())
                    .Bind( db => QueryEntities( db, action ).ToEither( Error.New ));
        }
        */
        /*
        private Try<Unit> SelectEntities( LiteDatabase db, Action<IEnumerable<T>> withAction ) {
            return Prelude.Try(  () => {
                withAction( Include( db.GetCollection<T>( mCollectionName )).FindAll());

                return Unit.Default;
            });
        }

        protected Either<Error, Unit> SelectEntities( Action<IEnumerable<T>> action ) {
            return ValidateAction( action )
                .Bind( _ => CreateConnection())
                    .Bind( db => SelectEntities( db, action ).ToEither( Error.New ));
        }
        */

        private Try<ILiteQueryable<T>> GetQueryable( LiteDatabase db ) {
            return Prelude.Try( () => Include( db.GetCollection<T>( mCollectionName )).Query());
        }

        protected Either<Error, ILiteQueryable<T>> GetList() { 
            return CreateConnection()
                .Bind( database => GetQueryable( database ).ToEither( Error.New ));
        }

        private TryOption<T> GetEntityById( LiteDatabase db, ObjectId id ) {
            return Prelude.TryOption( () => Include( db.GetCollection<T>( mCollectionName )).FindById( id ));
        }

        /*
        // returns option.IsNone if an entity is not located
        protected Either<Error, Option<T>> GetEntityById( ObjectId id ) {
            return ValidateObjectId( id )
                .Bind( _ => CreateConnection())
                    .Bind( db => GetEntityById( db, id ).ToEither( Error.New ));
        }
        */

        // returns an error if the entity is not located.
        protected Either<Error, T> GetEntityById( ObjectId id ) {
            return ValidateObjectId( id )
                .Bind( _ => CreateConnection())
                    .Bind( db => GetEntityById( db, id ).ToEither( Error.New ))
                        .Bind( x => x.IsNone ? Prelude.Left<Error, T>( Error.New( "Entity was not found by ID" ))
                                             : Prelude.Right<Error, T>( x.AsEnumerable().First()));
        }

        private Try<Unit> InsertEntity( LiteDatabase db, T entity ) {
            return Prelude.Try( () => {
                db.GetCollection<T>( mCollectionName ).Insert( entity );

                return Unit.Default;
            });
        }

        protected Either<Error, Unit> InsertEntity( T entity ) {
            return 
                from ve in ValidateEntity( entity )
                    from db in CreateConnection()
                        from ret in InsertEntity( db, ve ).ToEither( Error.New )
                            select ret;
        }

        private Try<Unit> UpdateEntity( LiteDatabase db, T entity ) {
            return Prelude.Try( () => {
                if(!db.GetCollection<T>( mCollectionName ).Update( entity )) {
                    throw new DatabaseException( "Entity to update was not found" );
                }

                return Unit.Default;
            });
        }

        protected Either<Error, Unit> UpdateEntity( T entity ) {
            return 
                from ve in ValidateEntity( entity )
                    from db in CreateConnection()
                        from ret in UpdateEntity( db, ve ).ToEither( Error.New )
                            select ret;
        }

        private Try<Unit> DeleteEntity( LiteDatabase db, T entity ) {
            return Prelude.Try( () => {
                if(!db.GetCollection<T>( mCollectionName ).Delete( entity.Id )) {
                    throw new DatabaseException( "Entity to delete was not found" );
                }

                return Unit.Default;
            });
        }

        protected Either<Error, Unit> DeleteEntity( T entity ) {
            return 
                from ve in ValidateEntity( entity )
                    from db in CreateConnection()
                        from ret in DeleteEntity( db, ve ).ToEither( Error.New )
                            select ret;
        }

        public void Dispose() {
            if( mDatabase != null ) {
                mDatabase.Dispose();
                mDatabase = null;
            }
        }
    }
}
