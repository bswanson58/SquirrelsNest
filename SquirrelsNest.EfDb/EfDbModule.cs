using Autofac;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.EfDb.Context;
using SquirrelsNest.EfDb.Providers;

namespace SquirrelsNest.EfDb {
    public class EfDbModule : Module {
        protected override void Load( ContainerBuilder builder ) {
            builder.RegisterType<SquirrelsNestDbContext>().InstancePerDependency();

            builder.RegisterType<ContextFactory>().As<ContextFactory>().SingleInstance();
            builder.RegisterType<ComponentProvider>().As<IComponentProvider>().InstancePerDependency();
        }
    }
}
