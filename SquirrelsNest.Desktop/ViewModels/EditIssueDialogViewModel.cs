using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MoreLinq;
using MvvmSupport.DialogService;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Logging;
using SquirrelsNest.Common.Values;
using SquirrelsNest.Desktop.Support;

namespace SquirrelsNest.Desktop.ViewModels {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class EditIssueDialogViewModel : DialogAwareBase {
        private readonly IIssueTypeProvider mIssueTypeProvider;
        private readonly ILog               mLog;

        public  const string                cIssueParameter = "issue";
        public  const string                cProjectParameter = "project";

        private SnIssue ?                   mIssue;
        private SnProject ?                 mProject;
        private string                      mTitle;
        private string                      mDescription;
        private SnIssueType                 mCurrentIssueType;

        public  ObservableCollection<SnIssueType>   IssueTypes { get; }
        public  string                              EntryInfo { get; private set; }

        public EditIssueDialogViewModel( IIssueTypeProvider issueTypeProvider, ILog log ) {
            mIssueTypeProvider = issueTypeProvider;
            mLog = log;

            mTitle = String.Empty;
            mDescription = String.Empty;
            IssueTypes = new ObservableCollection<SnIssueType>();
            mCurrentIssueType = SnIssueType.Default;

            SetTitle( "Issue Properties" );
            EntryInfo = String.Empty;
        }

        public override void OnDialogOpened( IDialogParameters parameters ) {
            mIssue = parameters.GetValue<SnIssue>( cIssueParameter );
            mProject = parameters.GetValue<SnProject>( cProjectParameter );

            if( mProject == null ) {
                throw new ApplicationException( "A project parameter is required" );
            }

            if( mIssue != null ) {
                IssueTitle = mIssue.Title;
                Description = mIssue.Description;

                EntryInfo = $"Entered on {mIssue.EntryDate.ToShortDateString()}";
                OnPropertyChanged( nameof( EntryInfo ));
            }

            mIssueTypeProvider
                .GetIssues( mProject ).Result
                .Map( list => list.OrderBy( it => it.Name ))
                .Map( list => Enumerable.Append( list, SnIssueType.Default ))
                .Do( list => list.ForEach( it => IssueTypes.Add( it )))
                .IfLeft( e => mLog.LogError( e ));

            CurrentIssueType = IssueTypes.FirstOrDefault( it => it.EntityId.Equals( mIssue != null ? mIssue.IssueTypeId : EntityId.Default )) ?? SnIssueType.Default;
        }

        [Required( ErrorMessage = "Issue title is required" )]
        [MinLength( 3, ErrorMessage = "Issue titles must be a minimum of 3 characters")]
        [MaxLength( 100, ErrorMessage = "Issue titles must be less than 100 characters" )]
        public string IssueTitle {
            get => mTitle;
            set => SetProperty( ref mTitle, value, true );
        }

        public string Description {
            get => mDescription;
            set => SetProperty( ref mDescription, value, true );
        }

        public SnIssueType CurrentIssueType {
            get => mCurrentIssueType;
            set => SetProperty( ref mCurrentIssueType, value, true );
        }

        protected override void OnAccept() {
            ValidateAllProperties();

            if((!HasErrors ) &&
               ( mProject != null )) {
                var issue = mIssue ?? new SnIssue( IssueTitle, mProject.NextIssueNumber, mProject.EntityId );

                issue = issue
                    .With( title: IssueTitle, description: Description )
                    .With( CurrentIssueType );

                RaiseRequestClose( 
                    new DialogResult( ButtonResult.Ok, 
                        new DialogParameters {{ cIssueParameter, issue }, { cProjectParameter, mProject }}));
            }
        }
    }
}
