using System;
using Fluxor;
using SquirrelsNest.Pecan.Client.ProjectTemplates.Actions;
using SquirrelsNest.Pecan.Client.ProjectTemplates.Store;

namespace SquirrelsNest.Pecan.Client.ProjectTemplates.Reducers {
    // ReSharper disable once UnusedType.Global
    public static class RequestProjectTemplatesReducer {
        [ReducerMethod( typeof( RequestProjectTemplatesAction ))]
        public static ProjectTemplatesState RequestProjectTemplates( ProjectTemplatesState state ) =>
            new( true, String.Empty, state.Templates );

        [ReducerMethod]
        public static ProjectTemplatesState RequestProjectTemplatesSuccess( ProjectTemplatesState state, 
                                                                            RequestProjectTemplatesSuccess action ) =>
            new( false, String.Empty, action.Templates );

        [ReducerMethod]
        public static ProjectTemplatesState RequestProjectTemplatesFailure( ProjectTemplatesState state, 
                                                                            RequestProjectTemplatesFailure action ) =>
            new( false, action.Message, state.Templates );
    }
}
