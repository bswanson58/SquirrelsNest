using System;

namespace SquirrelsNest.Pecan.Server.Features.ProjectTemplates {
    public class TemplateParameters {
        public  string  TemplateName { get; set; }
        public  string  TemplateDescription { get; set; }

        public TemplateParameters() {
            TemplateName = String.Empty;
            TemplateDescription = String.Empty;
        }
    }
}
