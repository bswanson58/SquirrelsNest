using System.Reactive.Linq;
using System.Reactive.Subjects;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Values;
using SquirrelsNest.EfDb.Context;
using SquirrelsNest.EfDb.Dto;

namespace SquirrelsNest.EfDb.Providers {
    internal class ComponentProvider : IComponentProvider {
        private readonly IContextFactory                mContextProvider;
        private readonly Subject<EntitySourceChange>    mChangeSubject;

        public IObservable<EntitySourceChange>          OnEntitySourceChange => mChangeSubject.AsObservable();

        public ComponentProvider( IContextFactory contextFactory ) {
            mContextProvider = contextFactory;

            mChangeSubject = new Subject<EntitySourceChange>();
        }

        private static SnComponent ConvertTo( DbComponent component ) => component.ToEntity();
        private static DbComponent ConvertFrom( SnComponent component ) => DbComponent.From( component );

        public async Task<Either<Error, SnComponent>> AddComponent( SnComponent component ) {
            try {
                await using var context = mContextProvider.ProvideContext();

                var retValue = ConvertFrom( component );

                await context.Components.AddAsync( retValue );
                await context.SaveChangesAsync();

                mChangeSubject.OnNext( EntitySourceChange.EntityInserted );

                return ConvertTo( retValue );
            }
            catch( Exception ex ) {
                return Error.New( ex );
            }
        }

        public async Task<Either<Error, Unit>> UpdateComponent( SnComponent component ) {
            try {
                await using var context = mContextProvider.ProvideContext();

                context.Components.Update( ConvertFrom( component ));
                await context.SaveChangesAsync();

                mChangeSubject.OnNext( EntitySourceChange.EntityUpdated );

                return Unit.Default;
            } 
            catch( Exception ex ) {
                return Error.New( ex );
            }
        }

        public async Task<Either<Error, Unit>> DeleteComponent( SnComponent component ) {
            try {
                await using var context = mContextProvider.ProvideContext();

                context.Components.Remove( ConvertFrom( component ));
                await context.SaveChangesAsync();

                mChangeSubject.OnNext( EntitySourceChange.EntityDeleted );

                return Unit.Default;
            } 
            catch( Exception ex ) {
                return Error.New( ex );
            }
        }

        public async Task<Either<Error, SnComponent>> GetComponent( EntityId componentId ) {
            try {
                await using var context = mContextProvider.ProvideContext();

                var dbComponent = await context.Components.FirstOrDefaultAsync( c => c.EntityId.Equals( componentId ));

                return dbComponent != null ? 
                    ConvertTo( dbComponent ) : 
                    Error.New( new ApplicationException( "Component could not be located" ));
            }
            catch( Exception ex ) {
                return Error.New( ex );
            }
        }

        public async Task<Either<Error, IEnumerable<SnComponent>>> GetComponents() {
            try {
                await using var context = mContextProvider.ProvideContext();

                var list = await context.Components.ToListAsync();

                return list.Select( ConvertTo ).ToSeq();
            }
            catch( Exception ex ) {
                return Error.New( ex );
            }
        }

        public async Task<Either<Error, IEnumerable<SnComponent>>> GetComponents( SnProject forProject ) {
            try {
                await using var context = mContextProvider.ProvideContext();

                var list = await context.Components.Where( c => c.ProjectId.Equals( forProject.EntityId )).ToListAsync();

                return list.Select( ConvertTo ).ToSeq();
            }
            catch( Exception ex ) {
                return Error.New( ex );
            }
        }

        public void Dispose() {
        }
    }
}
