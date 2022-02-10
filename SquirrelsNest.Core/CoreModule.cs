using Autofac;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Core.Environment;

namespace SquirrelsNest.Core {
    public class CoreModule : Module {
        protected override void Load( ContainerBuilder builder ) {
            builder.RegisterType<ApplicationConstants>().As<IApplicationConstants>();
            builder.RegisterType<ApplicationEnvironment>().As<IEnvironment>();
        }
    }
}
