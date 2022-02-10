using System;
using System.Reflection;
using Autofac.Core;

namespace SquirrelsNest.Desktop.Ioc {
    public interface IDependencyContainer {
        IDependencyContainer    RegisterModule( IModule module );
        IDependencyContainer    RegisterModule<T>() where T: IModule, new();
        IDependencyContainer    RegisterViewModels( Assembly forAssembly );
        IDependencyContainer    RegisterAsInterfaces( Assembly forAssembly, string classNameSuffix );
        IDependencyContainer    BuildDependencies();

        void    Stop();

        object  Resolve( Type t );
        T       Resolve<T>() where T: notnull;
        T       Resolve<T>( Parameter[] parameters ) where T : notnull;
    }
}
