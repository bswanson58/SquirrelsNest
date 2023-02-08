using System;
using Fluxor;
using SquirrelsNest.Pecan.Shared.Entities;
using System.Collections.Generic;
using System.Linq;
using SquirrelsNest.Pecan.Client.Store;
using SquirrelsNest.Pecan.Shared.Dto;

namespace SquirrelsNest.Pecan.Client.Issues.Store {
    [FeatureState( CreateInitialStateMethodName = "Factory" )]
    public class IssueState : RootState {
        public IReadOnlyList<SnCompositeIssue>  Issues { get; }
        public PageInformation                  PageInformation { get; }
        public string                           CurrentProjectId { get; }
        public uint                             CurrentDisplayPage { get; }

        public IssueState( bool callInProgress, string callMessage, 
                           IEnumerable<SnCompositeIssue> issues, PageInformation pageInformation,
                           string currentProjectId, uint currentDisplayPage ) :
            base( callInProgress, callMessage ) {
            Issues = new List<SnCompositeIssue>( issues );
            PageInformation = pageInformation;
            CurrentProjectId = currentProjectId;
            CurrentDisplayPage = currentDisplayPage;
        }

        public static IssueState Factory() => 
            new ( false, String.Empty, Enumerable.Empty<SnCompositeIssue>(), PageInformation.Default,
                  SnCompositeProject.Default.EntityId, 1 );
    }
}
