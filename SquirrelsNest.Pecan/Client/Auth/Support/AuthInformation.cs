using System;
using System.Security.Claims;
using Fluxor;
using SquirrelsNest.Pecan.Client.Auth.Store;
using SquirrelsNest.Pecan.Shared.Constants;
using SquirrelsNest.Pecan.Shared.Platform;

namespace SquirrelsNest.Pecan.Client.Auth.Support {
    public interface IAuthInformation {
        TimeSpan    TimeOffsetToTokenExpiration { get; }
        bool        IsAuthValid { get; }
        string      UserName { get; }
        string      UserEmail { get; }
        string      UserEmailHash { get; }
    }

    public class AuthInformation : IAuthInformation {
        private readonly IState<AuthState>      mAuthState;

        public AuthInformation( IState<AuthState> authState ) {
            mAuthState = authState;
        }

        public string UserName => 
            IsAuthValid ? 
                JwtParser.GetClaimValue( mAuthState.Value.UserToken, ClaimTypes.GivenName ) : 
                String.Empty;

        public string UserEmail =>
            IsAuthValid ?
                JwtParser.GetClaimValue( mAuthState.Value.UserToken, ClaimTypes.Email ) : 
                String.Empty;

        public string UserEmailHash =>
            IsAuthValid ?
                JwtParser.GetClaimValue( mAuthState.Value.UserToken, ClaimValues.ClaimEmailHash ) : 
                String.Empty;

        public bool IsAuthValid =>
            TimeOffsetToTokenExpiration > TimeSpan.Zero;

        public TimeSpan TimeOffsetToTokenExpiration {
            get {
                if(!String.IsNullOrWhiteSpace( mAuthState.Value.UserToken )) {
                    var expiration = JwtParser.GetClaimValue( mAuthState.Value.UserToken, "exp" );

                    if(!String.IsNullOrWhiteSpace( expiration )) {
                        var expTime = DateTimeOffset.FromUnixTimeSeconds( Convert.ToInt64( expiration ));
                        
                        return expTime - DateTimeProvider.Instance.CurrentUtcTime;
                    }
                }

                return TimeSpan.Zero;
            }
        }
    }
}
