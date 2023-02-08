using Autofac;
using SquirrelsNest.Common.Interfaces.Database;
using SquirrelsNest.EfDb.Context;
using SquirrelsNest.EfDb.Providers;
using SquirrelsNest.EfDb.Support;

namespace SquirrelsNest.EfDb {
    public class EfDbModule : Module {
        protected override void Load( ContainerBuilder builder ) {
            builder.RegisterType<ConfigurationBuilder>().SingleInstance();
            builder.RegisterType<ContextFactory>().As<IContextFactory>().SingleInstance();
//            builder.RegisterType<SquirrelsNestDbContext>().InstancePerDependency();
            builder.RegisterType<SnDatabaseInitializer>().As<IDatabaseInitializer>();

            builder.RegisterType<AssociationProvider>().As<IDbAssociationProvider>().SingleInstance();
            builder.RegisterType<ComponentProvider>().As<IDbComponentProvider>().SingleInstance();
            builder.RegisterType<IssueProvider>().As<IDbIssueProvider>().SingleInstance();
            builder.RegisterType<IssueTypeProvider>().As<IDbIssueTypeProvider>().SingleInstance();
            builder.RegisterType<ProjectProvider>().As<IDbProjectProvider>().SingleInstance();
            builder.RegisterType<ReleaseProvider>().As<IDbReleaseProvider>().SingleInstance();
            builder.RegisterType<WorkflowStateProvider>().As<IDbWorkflowStateProvider>().SingleInstance();
            builder.RegisterType<UserProvider>().As<IDbUserProvider>().SingleInstance();
            builder.RegisterType<UserDataProvider>().As<IDbUserDataProvider>().SingleInstance();
        }
    }
}
