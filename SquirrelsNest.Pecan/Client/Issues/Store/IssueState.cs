using Fluxor;
using SquirrelsNest.Pecan.Shared.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SquirrelsNest.Pecan.Client.Issues.Store {
    [FeatureState( CreateInitialStateMethodName = "Factory" )]
    public class IssueState {
        public IReadOnlyList<SnCompositeIssue>  Issues { get; }

        public IssueState( IEnumerable<SnCompositeIssue> issues ) {
            Issues = new List<SnCompositeIssue>( issues );
        }

        public static IssueState Factory() => new ( Enumerable.Empty<SnCompositeIssue>());
    }
}
