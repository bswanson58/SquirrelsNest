using Autofac;
using MvvmSupport.DialogService;
using SquirrelsNest.Desktop.Ioc;

namespace SquirrelsNest.Desktop {
    internal class DesktopModule : Module {
        protected override void Load( ContainerBuilder builder ) {
            builder.RegisterType<DialogServiceContainer>().As<IDialogServiceContainer>().SingleInstance();
        }
    }
}
