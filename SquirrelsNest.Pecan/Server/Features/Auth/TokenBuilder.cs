﻿using System;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using SquirrelsNest.Pecan.Server.Database.Entities;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SquirrelsNest.Pecan.Shared.Constants;
using SquirrelsNest.Pecan.Shared.Platform;

namespace SquirrelsNest.Pecan.Server.Features.Auth {
    public interface ITokenBuilder {
        Task<string>        GenerateToken( DbUser user );
        string              GenerateRefreshToken();
        DateTime            TokenExpiration();
        ClaimsPrincipal     GetPrincipalFromExpiredToken( string token );
    }

    public class TokenBuilder : ITokenBuilder {
        private readonly UserManager<DbUser>    mUserManager;
        private readonly IConfigurationSection  mJwtSettings;
        private readonly ILogger<TokenBuilder>  mLog;

        public TokenBuilder( UserManager<DbUser> userManager, IConfiguration configuration, ILogger<TokenBuilder> log ) {
            mUserManager = userManager;
            mLog = log;

            mJwtSettings = configuration.GetSection( JWTConstants.JwtConfigSettings );
        }

        private SigningCredentials GetSigningCredentials() {
            var key = Encoding.UTF8.GetBytes( mJwtSettings[JWTConstants.JwtConfigSecurityKey] ?? String.Empty );
            var secret = new SymmetricSecurityKey( key );

            return new SigningCredentials( secret, SecurityAlgorithms.HmacSha256 );
        }

        private async Task<List<Claim>> BuildUserClaims( DbUser user ) {
            var claims = new List<Claim> {
                new( ClaimTypes.Name, user.UserName ?? String.Empty ),
                new( ClaimValues.ClaimEntityId, user.Id ),
                new( ClaimTypes.Email, user.Email ?? String.Empty ),
                new( ClaimValues.ClaimEmailHash, user.Email?.CalculateMd5Hash() ?? String.Empty )
            };

            var dbClaims = await mUserManager.GetClaimsAsync( user );

            claims.AddRange( dbClaims );

            var dbRoles = await mUserManager.GetRolesAsync( user );

            claims.AddRange( dbRoles.Select( r => new Claim( ClaimTypes.Role, r ) ) );

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions( SigningCredentials signingCredentials, List<Claim> claims ) {
            var tokenOptions = new JwtSecurityToken(
                issuer: mJwtSettings[JWTConstants.JwtConfigIssuer],
                audience: mJwtSettings[JWTConstants.JwtConfigAudience],
                claims: claims,
                expires: TokenExpiration(),
                signingCredentials: signingCredentials);
            return tokenOptions;
        }

        public async Task<string> GenerateToken( DbUser user ) {
            var signingCredentials = GetSigningCredentials(); 
            var claims = await BuildUserClaims( user );
            var tokenOptions = GenerateTokenOptions( signingCredentials, claims );

            return new JwtSecurityTokenHandler().WriteToken( tokenOptions );
        }

        public string GenerateRefreshToken() {
            var randomNumber = new byte[32];

            using( var rng = RandomNumberGenerator.Create()) {
                rng.GetBytes( randomNumber );
                return Convert.ToBase64String( randomNumber );
            }
        }

        public DateTime TokenExpiration() =>
             DateTimeProvider.Instance.CurrentUtcTime.AddMinutes( 
                 Convert.ToDouble( mJwtSettings[JWTConstants.JwtConfigExpiration]));

        public static TokenValidationParameters CreateTokenValidationParameters( IConfigurationSection jwtSettings ) =>
            new TokenValidationParameters {
                ValidateAudience = false,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = jwtSettings[JWTConstants.JwtConfigIssuer],
                ValidAudience = jwtSettings[JWTConstants.JwtConfigAudience],
                IssuerSigningKey = 
                    new SymmetricSecurityKey( 
                        Encoding.UTF8.GetBytes( jwtSettings[JWTConstants.JwtConfigSecurityKey] ?? String.Empty )),
            };

        public ClaimsPrincipal GetPrincipalFromExpiredToken( string token ) {
            try {
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenParameters = CreateTokenValidationParameters( mJwtSettings );
                var principal = tokenHandler.ValidateToken( token, tokenParameters, out var securityToken );
                var jwtSecurityToken = securityToken as JwtSecurityToken;

                if(( jwtSecurityToken == null ) ||
                   (!jwtSecurityToken.Header.Alg.Equals( SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase ))) {
                    throw new SecurityTokenException( "Invalid token" );
                }

                return principal;
            }
            catch( Exception ex ) {
                mLog.LogError( ex, "GetPrincipalFromExpiredToken" );
            }

            return new ClaimsPrincipal( new ClaimsIdentity( new List<Claim>()));
        }
    }
}
