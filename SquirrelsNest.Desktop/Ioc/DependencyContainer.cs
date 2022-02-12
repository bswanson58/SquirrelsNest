using System;
using System.Reflection;
using Autofac;
using Autofac.Core;

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
            if( mRootScope != null ) {
                throw new ApplicationException( "All registrations must occur before building dependencies" );
            }

            mBuilder.RegisterModule( module );

            return this;
        }

        public IDependencyContainer RegisterModule<T>() where T: IModule, new() {
            if( mRootScope != null ) {
                throw new ApplicationException( "All registrations must occur before building dependencies" );
            }

            mBuilder.RegisterModule<T>();

            return this;
        }

        public IDependencyContainer RegisterViewModels( Assembly forAssembly ) {
            if( mRootScope != null ) {
                throw new ApplicationException( "All registrations must occur before building dependencies" );
            }

            mBuilder.RegisterAssemblyTypes( forAssembly )
                .Where( t => t.Name.EndsWith( "ViewModel" ));

            return this;
        }

        public IDependencyContainer RegisterAsInterfaces( Assembly forAssembly, string classNameSuffix ) {
            if( mRootScope != null ) {
                throw new ApplicationException( "All registrations must occur before building dependencies" );
            }

            mBuilder.RegisterAssemblyTypes( forAssembly )
                .Where( t => t.Name.EndsWith( classNameSuffix ))
                .SingleInstance()
                .AsImplementedInterfaces();

            return this;
        }

        public IDependencyContainer RegisterDialog( Type viewType, string viewName ) {
            if( mRootScope != null ) {
                throw new ApplicationException( "All registrations must occur before building dependencies" );
            }

            mBuilder.RegisterType( viewType ).Named( viewName, typeof( object ));

            return this;
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
