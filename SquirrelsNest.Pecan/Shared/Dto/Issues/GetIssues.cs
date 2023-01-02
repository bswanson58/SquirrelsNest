using SquirrelsNest.Pecan.Shared.Entities;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System;
using SquirrelsNest.Pecan.Shared.Constants;

namespace SquirrelsNest.Pecan.Shared.Dto.Issues {
    public class GetIssuesRequest {
        public const string Route = $"{Routes.BaseRoute}/getIssues";
    }

    public class GetIssuesResponse : BaseResponse {
        public List<SnCompositeIssue>   Issues { get; }

        [JsonConstructor]
        public GetIssuesResponse( bool succeeded, string message, List<SnCompositeIssue> issues ) :
            base( succeeded, message ) {
            Issues = issues;
        }

        public GetIssuesResponse() {
            Issues = new List<SnCompositeIssue>();
        }

        public GetIssuesResponse( List<SnCompositeIssue> issues ) {
            Issues = new List<SnCompositeIssue>( issues );
        }

        public GetIssuesResponse( Exception ex ) :
            base( ex ) {
            Issues = new List<SnCompositeIssue>();
        }
    }
}
