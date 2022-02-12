using Autofac;
using MvvmSupport.DialogService;

namespace MvvmSupport {
    public class MvvmSupportModule : Module {
        protected override void Load( ContainerBuilder builder ) {
            builder.RegisterType<DialogService.DialogService>().As<IDialogService>().SingleInstance();
            builder.RegisterType<DialogWindow>().As<IDialogWindow>().InstancePerDependency();
        }
    }
}
