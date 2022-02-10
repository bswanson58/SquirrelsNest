using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac.Core;

namespace SquirrelsNest.Desktop.Ioc {
    public interface IDependencyContainer {
        IDependencyContainer    RegisterModule( Type moduleClass );
        IDependencyContainer    RegisterModules( IEnumerable<Type> containerModules );
        IDependencyContainer    RegisterViewModels( Assembly forAssembly );
        IDependencyContainer    RegisterAsInterfaces( Assembly forAssembly, string classNameSuffix );
        IDependencyContainer    BuildDependencies();

        void    Stop();

        object  Resolve( Type t );
        T       Resolve<T>() where T: notnull;
        T       Resolve<T>( Parameter[] parameters ) where T : notnull;
    }
}
