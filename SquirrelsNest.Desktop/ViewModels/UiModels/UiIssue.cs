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
        public  SnIssueType         IssueType => mCompositeIssue.IssueType;

        public  string              IssueNumber => $"{mCompositeIssue.Project.IssuePrefix}-{Issue.IssueNumber}";
        public  string              Title => Issue.Title;
        public  string              Description => Issue.Description;

        public  IRelayCommand       Edit { get; }

        public UiIssue( CompositeIssue compositeIssue, Action<UiIssue> onEdit ) {
            mCompositeIssue = compositeIssue;
            mOnEdit = onEdit;

            Edit = new RelayCommand( OnEdit );
        }

        private void OnEdit() {
            mOnEdit( this );
        }
    }
}
