using System;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Desktop.ViewModels.UiModels {
    internal class UiIssue : ObservableObject {
        private readonly Action<UiIssue>    mOnEdit;

        public  SnIssue             Issue { get; }
        public  SnProject           Project { get; }
        public  SnIssueType         IssueType { get; }

        public  string              IssueNumber => $"{Project.IssuePrefix}-{Issue.IssueNumber}";
        public  string              Title => Issue.Title;
        public  string              Description => Issue.Description;

        public  IRelayCommand       Edit { get; }

        public UiIssue( SnProject project, SnIssue issue, SnIssueType issueType, Action<UiIssue> onEdit ) {
            Project = project;
            Issue = issue;
            IssueType = issueType;
            mOnEdit = onEdit;

            Edit = new RelayCommand( OnEdit );
        }

        private void OnEdit() {
            mOnEdit( this );
        }
    }
}
