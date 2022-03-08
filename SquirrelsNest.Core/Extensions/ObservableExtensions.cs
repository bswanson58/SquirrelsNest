using System.Reactive.Linq;
using LanguageExt;

// from: https://stackoverflow.com/a/50096940, with additions to handle exceptions.

namespace SquirrelsNest.Core.Extensions {
    public static class ObservableExtensions {
        public static IDisposable SubscribeAsync<T>( this IObservable<T> source, Func<Task> asyncAction, Action<Exception> handler ) {
            async Task<Unit> Wrapped( T t ) {
                try {
                    await asyncAction();
                }
                catch( Exception ex ) {
                    handler( ex );
                }

                return Unit.Default;
            }

            return source.Select( Wrapped ).Subscribe( _ => { }, handler );
        }

        public static IDisposable SubscribeAsync<T>(this IObservable<T> source, Func<T, Task> asyncAction, Action<Exception> handler ) {
            async Task<Unit> Wrapped( T t ) {
                try {
                    await asyncAction( t );
                }
                catch( Exception ex ) {
                    handler( ex );
                }

                return Unit.Default;
            }

            return source.Select( Wrapped ).Subscribe( _ => { }, handler );
        }
    }
}
