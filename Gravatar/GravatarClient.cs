using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using Gravatar.Models;
using Gravatar.Support;

// ReSharper disable StringLiteralTypo
// ReSharper disable IdentifierTypo

namespace Gravatar {
    public enum GravatarDefaultImage {
        None,
        MysteryPerson,
        IdentIcon,
        MonsterId,
        Wavatar,
        Retro,
        RoboHash,
        Blank
    }

    public interface IGravatarClient {
        Task<Entry>         GetProfile( string email );

        Task<MemoryStream>  GetImage( string email );
        Task<MemoryStream>  GetImage( string email, uint imageSize );
        Task<MemoryStream>  GetImage( string email, GravatarDefaultImage imageStyle, bool forceDefault );
        Task<MemoryStream>  GetImage( string email, GravatarDefaultImage imageStyle, bool forceDefault, uint imageSize );
    }

    public class GravatarClient : IGravatarClient, IDisposable {
        private const string        cGravatarUri = "https://en.gravatar.com/";
        private readonly HttpClient mHttpClient;

        public GravatarClient( HttpClient httpClient ) {
            mHttpClient = httpClient;
            mHttpClient.DefaultRequestHeaders.UserAgent.ParseAdd( "SecretSquirrelSoftware/1.0" );
        }

        public async Task<Entry> GetProfile( string email ) {
            if( String.IsNullOrWhiteSpace( email )) {
                throw new ArgumentException( "Email must be provided", nameof( email ));
            }

            var uri = new Uri( $"{cGravatarUri}{email.CalculateMd5Hash()}.json" );
            var response = await mHttpClient.GetFromJsonAsync<GravatarResponse>( uri );
            var entry = response?.Entry.FirstOrDefault();

            return entry ?? new Entry();
        }

        private static string ImageParameters( GravatarDefaultImage style, bool forceDefault, uint imageSize ) {
            var sizeParam = imageSize > 0 ? $"s={imageSize}" : String.Empty;

            if( style == GravatarDefaultImage.None ) {
                return $"?{sizeParam}";
            }

            var forceParam = forceDefault ? "&f=y" : String.Empty;
            var defaultParam = style switch {
                GravatarDefaultImage.Blank => "blank",
                GravatarDefaultImage.IdentIcon => "identicon",
                GravatarDefaultImage.MonsterId => "monsterid",
                GravatarDefaultImage.MysteryPerson => "mp",
                GravatarDefaultImage.Retro => "retro",
                GravatarDefaultImage.RoboHash => "robohash",
                GravatarDefaultImage.Wavatar => "wavatar",
                _ => String.Empty
            };

            return $"?{sizeParam}&d={defaultParam}{forceParam}";
        }

        public Task<MemoryStream> GetImage( string email ) => GetImage( email, GravatarDefaultImage.None, false, 0 );
        public Task<MemoryStream> GetImage( string email, uint imageSize ) => GetImage( email, GravatarDefaultImage.None, false, imageSize );
        public Task<MemoryStream> GetImage( string email, GravatarDefaultImage imageStyle, bool forceDefault ) => GetImage( email, imageStyle, forceDefault, 0 );

        public async Task<MemoryStream> GetImage( string email, GravatarDefaultImage defaultImage, bool forceDefault, uint imageSize ) {
            if( String.IsNullOrWhiteSpace( email )) {
                throw new ArgumentException( "Email must be provided", nameof( email ));
            }

            var retValue = new MemoryStream();
            var uri = new Uri($"{cGravatarUri}avatar/{email.CalculateMd5Hash()}{ImageParameters( defaultImage, forceDefault, imageSize )}");
            using var response = await mHttpClient.GetAsync( uri );

            if( response.IsSuccessStatusCode ) {
                await using var stream = await response.Content.ReadAsStreamAsync();

                await stream.CopyToAsync( retValue, 8192 );
            }

            return retValue;
        }

        public void Dispose() {
            mHttpClient.Dispose();
        }
    }
}
