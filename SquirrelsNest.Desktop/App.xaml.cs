using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using MvvmSupport;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Core;
using SquirrelsNest.Desktop.Ioc;
using SquirrelsNest.Desktop.Models;
using SquirrelsNest.EfDb;
using SquirrelsNest.LiteDb;

namespace SquirrelsNest.Desktop {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App {
        private readonly IDependencyContainer   mContainer;
        private ILog ?                          mLog;

        public App() {
            mContainer = new DependencyContainer();

            DispatcherUnhandledException += AppDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException +=CurrentDomainUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerUnobservedTaskException;
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

            mLog = mContainer.Resolve<ILog>();

            mContainer.Resolve<Startup>()
                .StartApplication(() => mContainer.Stop());
        }

        private void CurrentDomainUnhandledException( object sender, UnhandledExceptionEventArgs e ) {
            if( e.ExceptionObject is Exception exception ) {
                mLog?.LogException( "Application Domain unhandled exception", exception );
            }

            Shutdown( -1 );
        }

        private void AppDispatcherUnhandledException( object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e ) {
            if( Debugger.IsAttached ) {
                Clipboard.SetText( e.Exception.ToString());
            }

            mLog?.LogException( "Application Dispatcher unhandled exception", e.Exception );

            e.Handled = true;
            Shutdown( -1 );
        }

        private void TaskSchedulerUnobservedTaskException( object ? sender, UnobservedTaskExceptionEventArgs e ) {
            mLog?.LogException( "Task Scheduler unobserved exception", e.Exception );

            e.SetObserved(); 
        }
    }
}
