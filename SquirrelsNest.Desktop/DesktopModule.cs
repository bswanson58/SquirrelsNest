using Autofac;
using MvvmSupport.DialogService;
using MvvmSupport.Ioc;
using SquirrelsNest.Desktop.Ioc;
using SquirrelsNest.Desktop.Views;

namespace SquirrelsNest.Desktop {
    internal class DesktopModule : Module {
        protected override void Load( ContainerBuilder builder ) {
            builder.RegisterType<DialogServiceContainer>().As<IDialogServiceContainer>().SingleInstance();

            builder.RegisterDialog<EditProjectDialog>();
        }
    }
}
