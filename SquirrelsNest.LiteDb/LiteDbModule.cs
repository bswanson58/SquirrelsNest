using Autofac;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.LiteDb.Database;
using SquirrelsNest.LiteDb.Providers;

namespace SquirrelsNest.LiteDb {
    public class LiteDbModule : Module {
        protected override void Load( ContainerBuilder builder ) {
            builder.RegisterType<DatabaseProvider>().As<IDatabaseProvider>().SingleInstance();

            builder.RegisterType<IssueProviderAsync>().As<IIssueProvider>().SingleInstance();
            builder.RegisterType<ProjectProviderAsync>().As<IProjectProvider>().SingleInstance();
            builder.RegisterType<ReleaseProviderAsync>().As<IReleaseProvider>().SingleInstance();
        }
    }
}
