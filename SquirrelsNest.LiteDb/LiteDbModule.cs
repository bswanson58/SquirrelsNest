using Autofac;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.LiteDb.Database;
using SquirrelsNest.LiteDb.Providers;

namespace SquirrelsNest.LiteDb {
    public class LiteDbModule : Module {
        protected override void Load( ContainerBuilder builder ) {
            builder.RegisterType<DatabaseProvider>().As<IDatabaseProvider>().SingleInstance();

            builder.RegisterType<AsyncIssueProvider>().As<IIssueProvider>().SingleInstance();
            builder.RegisterType<ProjectProvider>().As<IProjectProvider>().SingleInstance();
        }
    }
}
