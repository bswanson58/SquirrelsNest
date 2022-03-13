using Autofac;
using FluentValidation;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Core.CompositeBuilders;
using SquirrelsNest.Core.Database;
using SquirrelsNest.Core.Environment;
using SquirrelsNest.Core.Interfaces;
using SquirrelsNest.Core.Platform;
using SquirrelsNest.Core.ProjectTemplates;
using SquirrelsNest.Core.Transfer.Export;
using SquirrelsNest.Core.Transfer.Import;
using SquirrelsNest.Core.Validators;

namespace SquirrelsNest.Core {
    public class CoreModule : Module {
        protected override void Load( ContainerBuilder builder ) {
            builder.RegisterType<ApplicationConstants>().As<IApplicationConstants>();
            builder.RegisterType<ApplicationEnvironment>().As<IEnvironment>();

            builder.RegisterType<FileWriter>().As<IFileWriter>().SingleInstance();

            builder.RegisterType<IssueBuilder>().As<IIssueBuilder>().SingleInstance();
            builder.RegisterType<ProjectBuilder>().As<IProjectBuilder>().SingleInstance();

            builder.RegisterType<ProjectTemplateSerializer>().As<IProjectTemplateSerializer>().InstancePerDependency();
            builder.RegisterType<ProjectTemplateManager>().As<IProjectTemplateManager>().InstancePerDependency();

            builder.RegisterType<ImportManager>().As<IImportManager>().SingleInstance();
            builder.RegisterType<ExportManager>().As<IExportManager>().SingleInstance();

            builder.RegisterType<CompositeProjectValidator>().As<IValidator<CompositeProject>>();

            // The entity providers
            builder.RegisterType<ComponentProvider>().As<IComponentProvider>().InstancePerDependency();
            builder.RegisterType<IssueProvider>().As<IIssueProvider>().InstancePerDependency();
            builder.RegisterType<IssueTypeProvider>().As<IIssueTypeProvider>().SingleInstance();
            builder.RegisterType<ProjectProvider>().As<IProjectProvider>().SingleInstance();
            builder.RegisterType<ReleaseProvider>().As<IReleaseProvider>().SingleInstance();
            builder.RegisterType<WorkflowStateProvider>().As<IWorkflowStateProvider>().SingleInstance();
            builder.RegisterType<UserProvider>().As<IUserProvider>().SingleInstance();
            builder.RegisterType<UserDataProvider>().As<IUserDataProvider>().SingleInstance();
        }
    }
}
