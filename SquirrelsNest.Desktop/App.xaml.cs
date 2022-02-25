using System.Reflection;
using System.Windows;
using MvvmSupport;
using SquirrelsNest.Core;
using SquirrelsNest.Desktop.Ioc;
using SquirrelsNest.Desktop.Models;
using SquirrelsNest.LiteDb;

namespace SquirrelsNest.Desktop {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App {
        private readonly IDependencyContainer   mContainer;

        public App() {
            mContainer = new DependencyContainer();
        }

        protected override void OnStartup( StartupEventArgs e ) {
            base.OnStartup( e );

            mContainer
                .RegisterModule<DesktopModule>()
                .RegisterModule<MvvmSupportModule>()
                .RegisterModule<CoreModule>()
                .RegisterModule<LiteDbModule>()
                .RegisterViewModels( Assembly.GetExecutingAssembly())
                .RegisterSynchronizationContext()
                .RegisterViewModelLocator()
                .BuildDependencies();

            mContainer.Resolve<Startup>()
                .StartApplication(() => mContainer.Stop());
        }
    }
}
