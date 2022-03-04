// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Gravatar.Models {
    public class Url {
        public string Value { get; set; }
        public string Title { get; set; }

        public Url() {
            Value = String.Empty;
            Title = String.Empty;
        }
    }
}

