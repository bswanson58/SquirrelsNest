using MoreLinq;
using SquirrelsNest.Common.Interfaces;

namespace SquirrelsNest.Core.ProjectTemplates {
    internal partial class ProjectTemplateManager : IProjectTemplateManager {
        private readonly IApplicationConstants      mAppConstants;
        private readonly IProjectTemplateSerializer mSerializer;
        private readonly IProjectProvider           mProjectProvider;
        private readonly IIssueTypeProvider         mIssueTypeProvider;
        private readonly IComponentProvider         mComponentProvider;
        private readonly IWorkflowStateProvider     mStateProvider;
        private readonly ILog                       mLog;

        public ProjectTemplateManager( IProjectProvider projectProvider, IIssueTypeProvider issueTypeProvider,
                                       IComponentProvider componentProvider, IWorkflowStateProvider stateProvider, ILog log,
                                       IProjectTemplateSerializer serializer, IApplicationConstants appConstants ) {
            mLog = log;
            mAppConstants = appConstants;
            mProjectProvider = projectProvider;
            mIssueTypeProvider = issueTypeProvider;
            mComponentProvider = componentProvider;
            mStateProvider = stateProvider;
            mSerializer = serializer;
        }

        public IEnumerable<ProjectTemplate> GetAvailableTemplates() {
            var retValue = new List<ProjectTemplate>();
            
            mSerializer.ScanTemplates( $"*{mAppConstants.ProjectTemplateExtension}" )
                .ForEach( fileName => mSerializer.LoadTemplate( fileName )
                    .Match( template => retValue.Add( template ), 
                            exception => mLog.LogException( $"Loading project template file from: '{fileName}'", exception )));

            return retValue;
        }
    }
}
