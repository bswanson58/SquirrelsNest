using System;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Desktop.ViewModels.UiModels {
    internal class UiIssue : ObservableObject {
        private readonly Action<UiIssue>    mOnEdit;

        public  SnIssue             Issue { get; }
        public  string              Title => Issue.Title;
        public  string              Description => Issue.Description;

        public  IRelayCommand       Edit { get; }

        public UiIssue( SnIssue issue, Action<UiIssue> onEdit ) {
            Issue = issue;
            mOnEdit = onEdit;

            Edit = new RelayCommand( OnEdit );
        }

        private void OnEdit() {
            mOnEdit( this );
        }
    }
}
