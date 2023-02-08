using Autofac;

namespace SquirrelsNest.EfDb.Context {
    internal interface IContextFactory {
        SquirrelsNestDbContext  ProvideContext();
    }

    internal class ContextFactory : IContextFactory {
        private readonly ILifetimeScope     mScope;

        public ContextFactory( ILifetimeScope scope ) {
            mScope = scope;
        }

        public SquirrelsNestDbContext ProvideContext() {
            return mScope.Resolve<SquirrelsNestDbContext>();
        }
    }
}
