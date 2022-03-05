using System.Reactive.Linq;
using System.Reactive.Subjects;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Values;
using SquirrelsNest.EfDb.Context;
using SquirrelsNest.EfDb.Dto;

namespace SquirrelsNest.EfDb.Providers {
    internal abstract class EntityProvider<TEntity, TDatabase> where TDatabase : DbBase {
        private readonly IContextFactory                mContextProvider;
        private readonly Subject<EntitySourceChange>    mChangeSubject;

        public IObservable<EntitySourceChange>          OnEntitySourceChange => mChangeSubject.AsObservable();

        protected EntityProvider( IContextFactory contextFactory ) {
            mContextProvider = contextFactory;

            mChangeSubject = new Subject<EntitySourceChange>();
        }

        protected abstract TEntity ConvertTo( TDatabase dto );
        protected abstract TDatabase ConvertFrom( TEntity entity );

        public async Task<Either<Error, TEntity>> AddEntity( TEntity entity ) {
            try {
                await using var context = mContextProvider.ProvideContext();

                var retValue = ConvertFrom( entity );

                await context.Set<TDatabase>().AddAsync( retValue );
                await context.SaveChangesAsync();

                mChangeSubject.OnNext( EntitySourceChange.EntityInserted );

                return ConvertTo( retValue );
            }
            catch( Exception ex ) {
                return Error.New( ex );
            }
        }

        public async Task<Either<Error, Unit>> UpdateEntity( TEntity entity ) {
            try {
                await using var context = mContextProvider.ProvideContext();

                context.Set<TDatabase>().Update( ConvertFrom( entity ));
                await context.SaveChangesAsync();

                mChangeSubject.OnNext( EntitySourceChange.EntityUpdated );

                return Unit.Default;
            } 
            catch( Exception ex ) {
                return Error.New( ex );
            }
        }

        public async Task<Either<Error, Unit>> DeleteEntity( TEntity entity ) {
            try {
                await using var context = mContextProvider.ProvideContext();

                context.Set<TDatabase>().Remove( ConvertFrom( entity ));
                await context.SaveChangesAsync();

                mChangeSubject.OnNext( EntitySourceChange.EntityDeleted );

                return Unit.Default;
            } 
            catch( Exception ex ) {
                return Error.New( ex );
            }
        }

        public async Task<Either<Error, TEntity>> GetEntity( EntityId componentId ) {
            try {
                await using var context = mContextProvider.ProvideContext();

                var dbComponent = await context.Set<TDatabase>().FirstOrDefaultAsync( c => c.EntityId.Equals( componentId ));

                return dbComponent != null ? 
                    ConvertTo( dbComponent ) : 
                    Error.New( new ApplicationException( "Component could not be located" ));
            }
            catch( Exception ex ) {
                return Error.New( ex );
            }
        }

        public async Task<Either<Error, IEnumerable<TEntity>>> GetEntities() {
            try {
                await using var context = mContextProvider.ProvideContext();

                var list = await context.Set<TDatabase>().ToListAsync();

                return list.Select( ConvertTo ).ToSeq();
            }
            catch( Exception ex ) {
                return Error.New( ex );
            }
        }

        public async Task<Either<Error, IEnumerable<TEntity>>> GetEntities( Func<TDatabase, bool> predicate ) {
            try {
                await using var context = mContextProvider.ProvideContext();

                return context.Set<TDatabase>()
                    .Where( predicate )
                    .Select( ConvertTo )
                    .ToList();
            }
            catch( Exception ex ) {
                return Error.New( ex );
            }
        }

        public void Dispose() {
        }
    }
}
