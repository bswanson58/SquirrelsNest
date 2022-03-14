using System;
using System.Threading.Tasks;
using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Desktop.Models {
    internal interface IModelState {
        Task        SetProject( SnProject project );
        Task        SetUser( SnUser user );

        CurrentState                CurrentState { get; }
        IObservable<CurrentState>   OnStateChange { get; }
    }
}
