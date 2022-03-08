using System.Net.Http;
using Autofac;
using Gravatar;
using MvvmSupport.DialogService;
using MvvmSupport.Ioc;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Platform;
using SquirrelsNest.Desktop.Ioc;
using SquirrelsNest.Desktop.Models;
using SquirrelsNest.Desktop.Platform;
using SquirrelsNest.Desktop.Preferences;
using SquirrelsNest.Desktop.Views;
using SquirrelsNest.EfDb.Context;

namespace SquirrelsNest.Desktop {
    internal class DesktopModule : Module {
        protected override void Load( ContainerBuilder builder ) {
            // Classes from SquirrelsNest.Common
            builder.RegisterInstance( DateTimeProvider.Instance ).As<ITimeProvider>();
            builder.RegisterType<PlatformLog>().As<ILog>().As<IApplicationLog>().SingleInstance();

            // Preferences support classes
            builder.RegisterType<PreferencesHandler>().As<IPreferencesHandler>().SingleInstance();
            // Preference classes
            builder.RegisterType<Preferences<AppState>>().As<IPreferences<AppState>>().SingleInstance();
            builder.RegisterType<Preferences<EfDatabaseConfiguration>>().As<IPreferences<EfDatabaseConfiguration>>();

            builder.RegisterType<Startup>().SingleInstance();
            builder.RegisterType<ModelState>().As<IModelState>().SingleInstance();

            // Dialog support classes
            builder.RegisterType<DialogServiceContainer>().As<IDialogServiceContainer>().SingleInstance();

            // Gravatars library
            builder.RegisterType<HttpClient>().InstancePerDependency();
            builder.RegisterType<GravatarClient>().As<IGravatarClient>().SingleInstance();

            // Dialogs
            builder.RegisterDialog<ConfirmationDialog>();
            builder.RegisterDialog<CreateProjectDialog>();
            builder.RegisterDialog<EditIssueDialog>();
            builder.RegisterDialog<EditIssueTypeDialog>();
            builder.RegisterDialog<EditComponentDialog>();
            builder.RegisterDialog<EditProjectDialog>();
            builder.RegisterDialog<EditProjectParametersDialog>();
            builder.RegisterDialog<EditReleaseDialog>();
            builder.RegisterDialog<EditWorkflowStepDialog>();
            builder.RegisterDialog<EditUserDialog>();
            builder.RegisterDialog<ExportProjectDialog>();
        }
    }
}
