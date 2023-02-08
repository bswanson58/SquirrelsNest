using Fluxor;
using SquirrelsNest.Pecan.Client.UserData.Actions;
using SquirrelsNest.Pecan.Client.UserData.Store;

namespace SquirrelsNest.Pecan.Client.UserData.Reducers {
    // ReSharper disable once UnusedType.Global
    public static class UserDataReducer {
        [ReducerMethod]
        public static UserDataState UpdateUserData( UserDataState state, RequestUserDataSuccess action ) {
            var listStyle = IssueListStyle.FullDetail;

            if(( action.UserData.IssueListDisplayStyle.Equals( IssueListStyle.TitleOnly )) ||
               ( action.UserData.IssueListDisplayStyle.Equals( IssueListStyle.TitleDescription )) ||
               ( action.UserData.IssueListDisplayStyle.Equals( IssueListStyle.FullDetail ))) {
                listStyle = action.UserData.IssueListDisplayStyle;
            }

            return new( action.UserData.CurrentProjectId,
                        action.UserData.DisplayCompletedIssues, 
                        action.UserData.DisplayCompletedIssuesLast,
                        action.UserData.DisplayOnlyMyAssignedIssues,
                        listStyle );
        }
            
    }
}
