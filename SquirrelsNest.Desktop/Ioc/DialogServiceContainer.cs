using Autofac;
using MvvmSupport.DialogService;

namespace SquirrelsNest.Desktop.Ioc {
    internal class DialogServiceContainer : IDialogServiceContainer {
        private readonly ILifetimeScope mContainer;

        public DialogServiceContainer( ILifetimeScope container ) {
            mContainer = container;
        }

        public T Resolve<T>() where T: notnull {
            return mContainer.Resolve<T>();
        }

        public T Resolve<T>( string name ) where T: notnull{
            return mContainer.ResolveNamed<T>( name );
        }
    }
}
