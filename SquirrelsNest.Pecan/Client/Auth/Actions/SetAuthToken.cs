namespace SquirrelsNest.Pecan.Client.Auth.Actions {
    public class SetAuthToken {
        public  string  Token {  get; }
        public  string  RefreshToken { get; }

        public SetAuthToken( string token, string refreshToken ) {
            Token = token;
            RefreshToken = refreshToken;
        }
    }
}
