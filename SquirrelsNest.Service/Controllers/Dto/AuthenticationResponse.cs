using System;

namespace SquirrelsNest.Service.Controllers.Dto {
    public class AuthenticationResponse {
        public  string      Token { get; set; }
        public  DateTime    Expiration { get; set; }

        public AuthenticationResponse() {
            Token = String.Empty;
            Expiration = DateTime.Today;
        }
    }
}
