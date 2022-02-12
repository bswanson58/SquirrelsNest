using System.Reflection;
using System.Windows;
using MvvmSupport;
using MvvmSupport.ViewModelLocator;
using SquirrelsNest.Core;
using SquirrelsNest.Desktop.Ioc;
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
                .BuildDependencies();

            ViewModelLocationProvider.SetDefaultViewModelFactory( vm => mContainer.Resolve( vm ));

            var window = new MainWindow();

            window.Closed += (_, _) => mContainer.Stop();

            window.Show();
        }
    }
}
