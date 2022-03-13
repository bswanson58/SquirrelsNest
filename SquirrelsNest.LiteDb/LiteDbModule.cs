using Autofac;
using SquirrelsNest.Common.Interfaces.Database;
using SquirrelsNest.LiteDb.Database;
using SquirrelsNest.LiteDb.Providers;

namespace SquirrelsNest.LiteDb {
    public class LiteDbModule : Module {
        protected override void Load( ContainerBuilder builder ) {
            builder.RegisterType<DatabaseProvider>().As<IDatabaseProvider>().SingleInstance();

            builder.RegisterType<AssociationProviderAsync>().As<IDbAssociationProvider>().SingleInstance();
            builder.RegisterType<ComponentProviderAsync>().As<IDbComponentProvider>().SingleInstance();
            builder.RegisterType<IssueProviderAsync>().As<IDbIssueProvider>().SingleInstance();
            builder.RegisterType<IssueTypeProviderAsync>().As<IDbIssueTypeProvider>().SingleInstance();
            builder.RegisterType<ProjectProviderAsync>().As<IDbProjectProvider>().SingleInstance();
            builder.RegisterType<ReleaseProviderAsync>().As<IDbReleaseProvider>().SingleInstance();
            builder.RegisterType<WorkflowStateProviderAsync>().As<IDbWorkflowStateProvider>().SingleInstance();
            builder.RegisterType<UserProviderAsync>().As<IDbUserProvider>().SingleInstance();
            builder.RegisterType<UserDataProviderAsync>().As<IDbUserDataProvider>().SingleInstance();
        }
    }
}
