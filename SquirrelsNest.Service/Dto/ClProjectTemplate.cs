using SquirrelsNest.Common.Entities;
using System;
using SquirrelsNest.Core.ProjectTemplates;

namespace SquirrelsNest.Service.Dto {
    public class ClProjectTemplate {
        public string Name { get; }
        public string Description { get; }

        public ClProjectTemplate( string name, string description ) {
            Name = name;
            Description = description;
        }
    }

    public static class ClProjectTemplateEx {
        public static ClProjectTemplate ToCl( this ProjectTemplate template ) {
            return new ClProjectTemplate( template.TemplateName, template.TemplateDescription );
        }
    }
}
