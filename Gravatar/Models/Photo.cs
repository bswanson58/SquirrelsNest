// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Gravatar.Models {
    public class Photo {
        public string Value { get; set; }
        public string Type { get; set; }

        public Photo() {
            Value = String.Empty;
            Type = String.Empty;
        }
    }
}
