using System;
using System.ComponentModel.DataAnnotations;

namespace SquirrelsNest.Service.Controllers.Dto {
    public class UserCredentials {
        [Required]
        [EmailAddress]
        public string   Email { get; set; }
        [Required]
        public string   Password { get; set; }

        public UserCredentials() {
            Email = String.Empty;
            Password = String.Empty;
        }
    }
}
