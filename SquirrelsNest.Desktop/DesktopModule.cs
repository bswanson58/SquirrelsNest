using Autofac;
using MvvmSupport.DialogService;
using MvvmSupport.Ioc;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Platform;
using SquirrelsNest.Desktop.Ioc;
using SquirrelsNest.Desktop.Models;
using SquirrelsNest.Desktop.Platform;
using SquirrelsNest.Desktop.Views;

namespace SquirrelsNest.Desktop {
    internal class DesktopModule : Module {
        protected override void Load( ContainerBuilder builder ) {
            builder.RegisterInstance( DateTimeProvider.Instance ).As<ITimeProvider>();
            builder.RegisterType<PlatformLog>().As<ILog>().As<IApplicationLog>().SingleInstance();

            builder.RegisterType<ModelState>().As<IModelState>().SingleInstance();

            builder.RegisterType<DialogServiceContainer>().As<IDialogServiceContainer>().SingleInstance();

            builder.RegisterDialog<EditIssueDialog>();
            builder.RegisterDialog<EditIssueTypeDialog>();
            builder.RegisterDialog<EditComponentDialog>();
            builder.RegisterDialog<EditProjectDialog>();
            builder.RegisterDialog<EditReleaseDialog>();
            builder.RegisterDialog<EditWorkflowStepDialog>();
            builder.RegisterDialog<EditUserDialog>();
        }
    }
}
