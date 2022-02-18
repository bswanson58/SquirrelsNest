using System;
using System.Reflection;
using System.Threading;
using Autofac;
using Autofac.Core;
using MvvmSupport.ViewModelLocator;

namespace SquirrelsNest.Desktop.Ioc {
    public class DependencyContainer : IDependencyContainer {
        private readonly ContainerBuilder   mBuilder;
        private ILifetimeScope ?            mRootScope;

        public DependencyContainer() {
            mBuilder = new ContainerBuilder();
        }

        public IDependencyContainer BuildDependencies() {
            if( mRootScope != null ) {
                mRootScope.Dispose();
                mRootScope = null;
            }

            mRootScope = mBuilder.Build();

            return this;
        }

        public IDependencyContainer RegisterModule( IModule module ) {
            InsureScopeIsNotBuilt();

            mBuilder.RegisterModule( module );

            return this;
        }

        public IDependencyContainer RegisterModule<T>() where T: IModule, new() {
            InsureScopeIsNotBuilt();

            mBuilder.RegisterModule<T>();

            return this;
        }

        public IDependencyContainer RegisterViewModels( Assembly forAssembly ) {
            InsureScopeIsNotBuilt();

            mBuilder
                .RegisterAssemblyTypes( forAssembly )
                .Where( t => t.Name.EndsWith( "ViewModel" ));

            return this;
        }

        public IDependencyContainer RegisterAsInterfaces( Assembly forAssembly, string classNameSuffix ) {
            InsureScopeIsNotBuilt();

            mBuilder
                .RegisterAssemblyTypes( forAssembly )
                .Where( t => t.Name.EndsWith( classNameSuffix ))
                .SingleInstance()
                .AsImplementedInterfaces();

            return this;
        }

        public IDependencyContainer RegisterDialog( Type viewType, string viewName ) {
            InsureScopeIsNotBuilt();

            mBuilder.RegisterType( viewType ).Named( viewName, typeof( object ));

            return this;
        }

        public IDependencyContainer RegisterSynchronizationContext() {
            InsureScopeIsNotBuilt();

            if( SynchronizationContext.Current == null ) {
                throw new ApplicationException( "A null SynchronizationContext can not be registered" );
            }

            mBuilder.RegisterInstance( SynchronizationContext.Current );

            return this;
        }

        public IDependencyContainer RegisterViewModelLocator() {
            ViewModelLocationProvider.SetDefaultViewModelFactory( CreateViewModel );

            return this;
        }

        private object CreateViewModel( Type vmType ) {
            if( mRootScope == null ) {
                throw new ApplicationException( "Context has not been built before attempting to resolve a view model" );
            }

            return mRootScope.Resolve( vmType );
        }

        private void InsureScopeIsNotBuilt() {
            if( mRootScope != null ) {
                throw new ApplicationException( "All registrations must occur before building dependencies" );
            }
        }

        public void Stop() {
            mRootScope?.Dispose();
            mRootScope = null;
        }

        public object Resolve( Type t ) {
            if( mRootScope == null ) {
                throw new ApplicationException( "Dependency container hasn't been built" );
            }

            return mRootScope.Resolve( t );
        }

        public T Resolve<T>() where T: notnull {
            return Resolve<T>( Array.Empty<Parameter>());
        }

        public T Resolve<T>( Parameter[] parameters ) where T : notnull {
            if( mRootScope == null ) {
                throw new ApplicationException( "Dependency container hasn't been built" );
            }

            return mRootScope.Resolve<T>( parameters );
        }
    }
}
