﻿using Autofac;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.LiteDb.Database;
using SquirrelsNest.LiteDb.Providers;

namespace SquirrelsNest.LiteDb {
    public class LiteDbModule : Module {
        protected override void Load( ContainerBuilder builder ) {
            builder.RegisterType<DatabaseProvider>().As<IDatabaseProvider>().SingleInstance();

            builder.RegisterType<ComponentProviderAsync>().As<IComponentProvider>().SingleInstance();
            builder.RegisterType<IssueProviderAsync>().As<IIssueProvider>().SingleInstance();
            builder.RegisterType<IssueTypeProviderAsync>().As<IIssueTypeProvider>().SingleInstance();
            builder.RegisterType<ProjectProviderAsync>().As<IProjectProvider>().SingleInstance();
            builder.RegisterType<ReleaseProviderAsync>().As<IReleaseProvider>().SingleInstance();
            builder.RegisterType<WorkflowStateProviderAsync>().As<IWorkflowStateProvider>().SingleInstance();
            builder.RegisterType<UserProviderAsync>().As<IUserProvider>().SingleInstance();
        }
    }
}
