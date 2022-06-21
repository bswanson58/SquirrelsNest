using Autofac;
using SquirrelsNest.Service.Database;
using SquirrelsNest.Service.Issues;
using SquirrelsNest.Service.Projects;
using SquirrelsNest.Service.Users;

namespace SquirrelsNest.Service {
    public class ServiceModule : Module {
        protected override void Load( ContainerBuilder builder ) {
            builder.RegisterType<Authentication>().InstancePerDependency();
            builder.RegisterType<IssueQuery>().InstancePerDependency();
            builder.RegisterType<IssueMutations>().InstancePerDependency();
            builder.RegisterType<ProjectQuery>().InstancePerDependency();

            builder.RegisterType<DatabaseInitializer>();
        }
    }
}
