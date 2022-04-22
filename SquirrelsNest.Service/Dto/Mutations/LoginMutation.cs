using System;
using System.ComponentModel.DataAnnotations;

namespace SquirrelsNest.Service.Dto.Mutations {
    // ReSharper disable once ClassNeverInstantiated.Global
    public class LoginInput {
        [Required]
        [EmailAddress]
        public string   Email { get; set; }
        [Required]
        public string   Password { get; set; }

        public LoginInput() {
            Email = String.Empty;
            Password = String.Empty;
        }
    }

    public class LoginPayload {
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public  string      Token { get; set; }
        public  DateTime    Expiration { get; set; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global

        public LoginPayload() {
            Token = String.Empty;
            Expiration = DateTime.Today;
        }
    }
}
