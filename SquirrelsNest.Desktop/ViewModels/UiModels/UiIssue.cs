using System;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Core.CompositeBuilders;

namespace SquirrelsNest.Desktop.ViewModels.UiModels {
    internal class UiIssue : ObservableObject {
        private readonly Action<UiIssue>    mOnEdit;
        private readonly CompositeIssue     mCompositeIssue;

        public  SnIssue             Issue => mCompositeIssue.Issue;
        public  SnProject           Project { get; }
        public  SnIssueType         IssueType => mCompositeIssue.IssueType;

        public  string              IssueNumber => $"{Project.IssuePrefix}-{Issue.IssueNumber}";
        public  string              Title => Issue.Title;
        public  string              Description => Issue.Description;

        public  IRelayCommand       Edit { get; }

        public UiIssue( SnProject project, CompositeIssue compositeIssue, Action<UiIssue> onEdit ) {
            Project = project;
            mCompositeIssue = compositeIssue;
            mOnEdit = onEdit;

            Edit = new RelayCommand( OnEdit );
        }

        private void OnEdit() {
            mOnEdit( this );
        }
    }
}
