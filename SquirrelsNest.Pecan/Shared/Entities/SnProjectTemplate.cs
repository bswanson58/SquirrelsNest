using System.Text.Json.Serialization;

namespace SquirrelsNest.Pecan.Shared.Entities {
    public class SnProjectTemplate {
        public  string  TemplateName { get; }
        public  string  TemplateDescription { get; }

        [JsonConstructor]
        public SnProjectTemplate( string templateName, string templateDescription ) {
            TemplateName = templateName;
            TemplateDescription = templateDescription;
        }

        public SnProjectTemplate( SnProject fromProject ) {
            TemplateName = fromProject.Name;
            TemplateDescription = fromProject.Description;
        }
    }
}
