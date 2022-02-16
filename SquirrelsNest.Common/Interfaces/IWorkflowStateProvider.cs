using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Values;

namespace SquirrelsNest.Common.Interfaces {
    public interface IWorkflowStateProvider : IEntityChangeNotifier, IDisposable {
        Task<Either<Error, SnWorkflowState>>                AddState( SnWorkflowState state );
        Task<Either<Error, Unit>>                           UpdateState( SnWorkflowState state );
        Task<Either<Error, Unit>>                           DeleteState( SnWorkflowState state );

        Task<Either<Error, SnWorkflowState>>                GetState( EntityId stateId );
        Task<Either<Error, IEnumerable<SnWorkflowState>>>   GetStates();
    }
}
