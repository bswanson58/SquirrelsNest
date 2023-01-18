using System.Collections.Generic;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Server.Features.Transfer.Dto {
    internal class TransferMap {
        public  SnProject                   Project {  get; }

        private readonly Dictionary<string, string> mComponentMap;
        private readonly Dictionary<string, string> mIssueTypeMap;
        private readonly Dictionary<string, string> mStateMap;
        private readonly Dictionary<string, string> mReleaseMap;
        private readonly Dictionary<string, string> mUserMap;

        public TransferMap( SnProject forProject ) {
            Project = forProject;

            mComponentMap = new Dictionary<string, string>();
            mIssueTypeMap = new Dictionary<string, string>();
            mStateMap = new Dictionary<string, string>();
            mReleaseMap = new Dictionary<string, string>();
            mUserMap = new Dictionary<string, string>();
        }

        public void Add( TrComponent imported, SnComponent created ) {
            mComponentMap.Add( imported.EntityId, created.EntityId );
        }

        public void Add( TrIssueType imported, SnIssueType created ) {
            mIssueTypeMap.Add( imported.EntityId, created.EntityId );
        }

        public void Add( TrWorkflowState imported, SnWorkflowState created ) {
            mStateMap.Add( imported.EntityId, created.EntityId );
        }

        public void Add( TrRelease imported, SnRelease created ) {
            mReleaseMap.Add( imported.EntityId, created.EntityId );
        }

        public void Add( TrUser imported, SnUser created ) {
            mUserMap.Add( imported.EntityId, created.EntityId );
        }

        public SnIssue Convert( TrIssue imported ) {
            imported.ComponentId = mComponentMap.ContainsKey( imported.ComponentId ) ?
                mComponentMap[imported.ComponentId] :
                SnComponent.Default.EntityId;

            imported.IssueTypeId = mIssueTypeMap.ContainsKey( imported.IssueTypeId ) ?
                mIssueTypeMap[imported.IssueTypeId] :
                SnIssueType.Default.EntityId;
            
            imported.WorkflowStateId = mStateMap.ContainsKey( imported.WorkflowStateId ) ?
                mStateMap[imported.WorkflowStateId] :
                SnWorkflowState.Default.EntityId;
            
            imported.ReleaseId = mReleaseMap.ContainsKey( imported.ReleaseId ) ?
                mReleaseMap[imported.ReleaseId] :
                SnRelease.Default.EntityId;
            
            imported.EnteredById = mUserMap.ContainsKey( imported.EnteredById ) ?
                mUserMap[imported.EnteredById] :
                SnUser.Default.EntityId;
            
            imported.AssignedToId = mUserMap.ContainsKey( imported.AssignedToId ) ?
                mUserMap[imported.AssignedToId] :
                SnUser.Default.EntityId;

            return imported.ToNewEntity( Project );
        }
    }
}
