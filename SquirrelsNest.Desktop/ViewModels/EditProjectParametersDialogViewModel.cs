using System;
using System.ComponentModel.DataAnnotations;
using MvvmSupport.DialogService;
using SquirrelsNest.Core.ProjectTemplates;
using SquirrelsNest.Desktop.Support;

namespace SquirrelsNest.Desktop.ViewModels {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class EditProjectParametersDialogViewModel : DialogAwareBase {
        public  const string            cTemplate = "template";

        private TemplateParameters ?    mParameters;
        private string                  mName;
        private string                  mDescription;

        public EditProjectParametersDialogViewModel() {

            SetTitle( "Project Template Properties");

            mName = String.Empty;
            mDescription = String.Empty;
        }

        public override void OnDialogOpened( IDialogParameters parameters ) {
            mParameters = parameters.GetValue<TemplateParameters>( cTemplate );

            if( mParameters != null ) {
                Name = mParameters.TemplateName;
                Description = mParameters.TemplateDescription;
            }
        }

        [Required( ErrorMessage = "Name is required" )]
        [MinLength( 3, ErrorMessage = "Template names must be a minimum of 3 characters")]
        [MaxLength( 100, ErrorMessage = "Template names must be less than 100 characters" )]
        public string Name {
            get => mName;
            set => SetProperty( ref mName, value, true );
        }

        public string Description {
            get => mDescription;
            set => SetProperty( ref mDescription, value, true );
        }

        protected override void OnAccept() {
            ValidateAllProperties();

            if(!HasErrors ) {
                var template = mParameters ?? new TemplateParameters();

                template.TemplateName = Name;
                template.TemplateDescription = Description;

                RaiseRequestClose( new DialogResult( ButtonResult.Ok, new DialogParameters {{ cTemplate, template }}));
            }
        }
    }
}
