using System;
using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Desktop.Models {
    internal interface IModelState {
        void    SetProject( SnProject project );
        void    ClearProject();

        IObservable<CurrentState> OnStateChange { get; }
    }
}
