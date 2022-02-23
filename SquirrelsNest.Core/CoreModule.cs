using Autofac;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Core.CompositeBuilders;
using SquirrelsNest.Core.Environment;
using SquirrelsNest.Core.Interfaces;

namespace SquirrelsNest.Core {
    public class CoreModule : Module {
        protected override void Load( ContainerBuilder builder ) {
            builder.RegisterType<ApplicationConstants>().As<IApplicationConstants>();
            builder.RegisterType<ApplicationEnvironment>().As<IEnvironment>();

            builder.RegisterType<IssueBuilder>().As<IIssueBuilder>().SingleInstance();
        }
    }
}
