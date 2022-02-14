using System;
using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Desktop.Models {
    internal interface IModelState {
        void    SetProject( SnProject project );
        void    ClearProject();

        CurrentState                CurrentState { get; }
        IObservable<CurrentState>   OnStateChange { get; }
    }
}
