using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Core.Interfaces;

namespace SquirrelsNest.Core.CompositeBuilders {
    internal class IssueBuilder : IIssueBuilder {
        private readonly IProjectProvider       mProjectProvider;
        private readonly IIssueTypeProvider     mIssueTypeProvider;

        public IssueBuilder( IProjectProvider projectProvider, IIssueTypeProvider issueTypeProvider ) {
            mProjectProvider = projectProvider;
            mIssueTypeProvider = issueTypeProvider;
        }

        private SnProject GetProject( SnIssue forIssue ) {
            return mProjectProvider
                .GetProject( forIssue.ProjectId ).Result
                .IfLeft( SnProject.Default );
        }

        private SnIssueType GetIssueType( SnIssue forIssue ) {
            if( forIssue == null ) {
                throw new ArgumentNullException( nameof( forIssue ));
            }

            return mIssueTypeProvider
                .GetIssue( forIssue.IssueTypeId ).Result
                .IfLeft( SnIssueType.Default );
        }

        public CompositeIssue BuildCompositeIssue( SnIssue forIssue ) {
            return new CompositeIssue( GetProject( forIssue ), forIssue, GetIssueType( forIssue ));
        }
    }
}
