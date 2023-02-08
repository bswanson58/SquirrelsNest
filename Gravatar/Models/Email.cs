// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Gravatar.Models {
    public class Email {
        public string Primary { get; set; }
        public string Value { get; set; }

        public Email() {
            Primary = String.Empty;
            Value = String.Empty;
        }
    }
}
