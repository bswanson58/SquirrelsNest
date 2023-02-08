using Autofac;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Service.Database;
using SquirrelsNest.Service.Issues;
using SquirrelsNest.Service.Projects;
using SquirrelsNest.Service.Support;
using SquirrelsNest.Service.Users;

namespace SquirrelsNest.Service {
    public class ServiceModule : Module {
        protected override void Load( ContainerBuilder builder ) {
            builder.RegisterType<WebPlatformLog>().As<ILog>().As<IApplicationLog>().SingleInstance();

            builder.RegisterType<Authentication>().InstancePerDependency();
            builder.RegisterType<IssueQuery>().InstancePerDependency();
            builder.RegisterType<IssueMutations>().InstancePerDependency();
            builder.RegisterType<ProjectQuery>().InstancePerDependency();

            builder.RegisterType<IdentityDatabaseInitializer>();
        }
    }
}
