using System;
using Fluxor;
using SquirrelsNest.Pecan.Shared.Entities;
using System.Collections.Generic;
using System.Linq;
using SquirrelsNest.Pecan.Client.Store;

namespace SquirrelsNest.Pecan.Client.Issues.Store {
    [FeatureState( CreateInitialStateMethodName = "Factory" )]
    public class IssueState : RootState {
        public IReadOnlyList<SnCompositeIssue>  Issues { get; }

        public IssueState( bool callInProgress, string callMessage, IEnumerable<SnCompositeIssue> issues ) :
            base( callInProgress, callMessage ) {
            Issues = new List<SnCompositeIssue>( issues );
        }

        public static IssueState Factory() => new ( false, String.Empty, Enumerable.Empty<SnCompositeIssue>());
    }
}
