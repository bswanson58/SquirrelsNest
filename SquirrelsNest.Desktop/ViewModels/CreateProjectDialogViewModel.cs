﻿using System;
using System.ComponentModel.DataAnnotations;
using MvvmSupport.DialogService;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Core.ProjectTemplates;
using SquirrelsNest.Desktop.Platform;
using SquirrelsNest.Desktop.Support;

namespace SquirrelsNest.Desktop.ViewModels {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class CreateProjectDialogViewModel : DialogAwareBase {
        public  const string                        cProject = "project";
        public  const string                        cTemplate = "template";

        private readonly IProjectTemplateManager    mProjectTemplateManager;
        private SnProject ?                         mProject;
        private string                              mName;
        private string                              mIssuePrefix;
        private string                              mDescription;
        private ProjectTemplate ?                   mCurrentTemplate;

        public  RangeCollection<ProjectTemplate>    ProjectTemplates { get; }
        public  string                              TemplateDescription => mCurrentTemplate != null ? mCurrentTemplate.TemplateDescription : String.Empty;

        public CreateProjectDialogViewModel( IProjectTemplateManager projectTemplateManager ) {
            mProjectTemplateManager = projectTemplateManager;

            SetTitle( "Project Properties");

            ProjectTemplates = new RangeCollection<ProjectTemplate>();

            mName = String.Empty;
            mIssuePrefix = String.Empty;
            mDescription = String.Empty;
            mCurrentTemplate = null;
        }

        public override void OnDialogOpened( IDialogParameters parameters ) {
            mProject = parameters.GetValue<SnProject>( cProject );

            if( mProject != null ) {
                Name = mProject.Name;
                IssuePrefix = mProject.IssuePrefix;
                Description = mProject.Description;
            }

            ProjectTemplates.Reset( mProjectTemplateManager.GetAvailableTemplates());
        }

        [Required( ErrorMessage = "Name is required" )]
        [MinLength( 3, ErrorMessage = "Project names must be a minimum of 3 characters")]
        [MaxLength( 100, ErrorMessage = "Project names must be less than 100 characters" )]
        public string Name {
            get => mName;
            set => SetProperty( ref mName, value, true );
        }

        [Required]
        [MinLength(1)]
        [MaxLength(8)]
        public string IssuePrefix {
            get => mIssuePrefix;
            set => SetProperty( ref mIssuePrefix, value, true );
        }

        public string Description {
            get => mDescription;
            set => SetProperty( ref mDescription, value, true );
        }

        public ProjectTemplate ?  CurrentTemplate {
            get => mCurrentTemplate;
            set {
                SetProperty( ref mCurrentTemplate, value, true );

                OnPropertyChanged( nameof( TemplateDescription ));
            }
        }

        protected override void OnAccept() {
            ValidateAllProperties();

            if(!HasErrors ) {
                var project = mProject ?? new SnProject( Name, IssuePrefix );
                var parameters = new DialogParameters {
                    { cProject, project.With( name: Name, description: Description, issuePrefix: IssuePrefix ) }};

                if( mCurrentTemplate != null ) {
                    parameters.Add( cTemplate, mCurrentTemplate );
                }

                RaiseRequestClose( new DialogResult( ButtonResult.Ok, parameters ));
            }
        }
    }
}
