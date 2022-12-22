namespace SquirrelsNest.Pecan.Client.Auth.Actions {
    public class SetAuthToken {
        public  string  Token {  get; }

        public SetAuthToken( string token ) {
            Token = token;
        }
    }
}
