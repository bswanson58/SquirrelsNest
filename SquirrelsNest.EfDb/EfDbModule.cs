using Autofac;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.EfDb.Context;
using SquirrelsNest.EfDb.Providers;

namespace SquirrelsNest.EfDb {
    public class EfDbModule : Module {
        protected override void Load( ContainerBuilder builder ) {
            builder.RegisterType<ConfigurationBuilder>().SingleInstance();
            builder.RegisterType<ContextFactory>().As<IContextFactory>().SingleInstance();
            builder.RegisterType<SquirrelsNestDbContext>().InstancePerDependency();

            builder.RegisterType<ComponentProvider>().As<IComponentProvider>().SingleInstance();
            builder.RegisterType<IssueProvider>().As<IIssueProvider>().SingleInstance();
            builder.RegisterType<IssueTypeProvider>().As<IIssueTypeProvider>().SingleInstance();
            builder.RegisterType<ProjectProvider>().As<IProjectProvider>().SingleInstance();
            builder.RegisterType<ReleaseProvider>().As<IReleaseProvider>().SingleInstance();
            builder.RegisterType<WorkflowStateProvider>().As<IWorkflowStateProvider>().SingleInstance();
            builder.RegisterType<UserProvider>().As<IUserProvider>().SingleInstance();
        }
    }
}
