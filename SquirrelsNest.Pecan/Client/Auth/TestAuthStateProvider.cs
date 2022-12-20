using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

namespace SquirrelsNest.Pecan.Client.Auth {
    public class TestAuthStateProvider : AuthenticationStateProvider {

        public override async Task<AuthenticationState> GetAuthenticationStateAsync() {
            var claims = new List<Claim>() {
                new( ClaimTypes.Name, "Bill" ),
                new( ClaimTypes.Role, "Administrator" )
            };
            var anonymous = new ClaimsIdentity( claims, "TestAuthType" );
//            var anonymous = new ClaimsIdentity();

            await Task.Delay( 1500 );

            return await Task.FromResult( new AuthenticationState( new ClaimsPrincipal( anonymous )));
        }
    }
}
