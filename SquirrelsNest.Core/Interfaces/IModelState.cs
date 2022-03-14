using SquirrelsNest.Common.Entities;
using SquirrelsNest.Core.Models;

namespace SquirrelsNest.Core.Interfaces {
    public interface IModelState {
        Task        SetProject( SnProject project );
        Task        SetUser( SnUser user );

        CurrentState                CurrentState { get; }
        IObservable<CurrentState>   OnStateChange { get; }
    }
}
