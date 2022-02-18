using System.Reflection;
using System.Windows;
using LanguageExt;
using LanguageExt.SomeHelp;
using MvvmSupport;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Core;
using SquirrelsNest.Desktop.Ioc;
using SquirrelsNest.Desktop.Views;
using SquirrelsNest.LiteDb;

namespace SquirrelsNest.Desktop {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App {
        private readonly IDependencyContainer   mContainer;
        private Option<IApplicationLog>         mLog;

        public App() {
            mContainer = new DependencyContainer();
            mLog = Option<IApplicationLog>.None;
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

            mLog = mContainer.Resolve<IApplicationLog>().ToSome();
            mLog.Do( log => log.ApplicationStarting());

            var window = new MainWindow();

            window.Closed += (_, _) => {
                mContainer.Stop();

                mLog.Do( log => log.ApplicationExiting());
            };

            window.Show();
        }
    }
}
