// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

using System;

namespace SquirrelsNest.Pecan.Client.Gravatar.Models {
    internal class GravatarResponse {
        public Entry[] Entry { get; set; }

        public GravatarResponse() {
            Entry = Array.Empty<Entry>();
        }
    }
}
