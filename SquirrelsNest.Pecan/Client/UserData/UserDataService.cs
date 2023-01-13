using System;
using System.Text.Json;
using System.Threading.Tasks;
using SquirrelsNest.Pecan.Client.Support;
using SquirrelsNest.Pecan.Shared.Dto.UserData;

namespace SquirrelsNest.Pecan.Client.UserData {
    public interface IUserDataService {
        Task<PecanUserData>     RequestUserData();
        Task                    PersistUserData( PecanUserData userData );
    }

    public class UserDataService : IUserDataService {
        private const string                        UserDataType = "Pecan";

        private readonly IAuthenticatedHttpHandler  mHttpHandler;

        public UserDataService( IAuthenticatedHttpHandler httpHandler ) {
            mHttpHandler = httpHandler;
        }

        public async Task<PecanUserData> RequestUserData() {
            var request = new GetUserDataRequest( UserDataType );
            var response = await mHttpHandler.Post<GetUserDataResponse>( GetUserDataRequest.Route, request );

            if(( response?.UserData != null ) &&
               ( response.Succeeded ) &&
               ( response.DataType.Equals( UserDataType ))) {
                if(!String.IsNullOrWhiteSpace( response.UserData )) {
                    return JsonSerializer.Deserialize<PecanUserData>( response.UserData ) ?? new PecanUserData();
                }

                return new PecanUserData();
            }

            return new PecanUserData();
        }

        public async Task PersistUserData( PecanUserData userData ) {
            var jsonData = JsonSerializer.Serialize( userData );
            var request = new UpdateUserDataRequest( UserDataType, jsonData );
            var response = await mHttpHandler.Post<UpdateUserDataResponse>( UpdateUserDataRequest.Route, request );
/*
            if(( response?.UserData != null ) &&
               ( response.Succeeded ) &&
               ( response.UserData.DataType.Equals( UserDataType ))) {
                var pecanData = !string.IsNullOrWhiteSpace( response.UserData.Data ) ?
                                    JsonSerializer.Deserialize<PecanUserData>( response.UserData.Data ) :
                                    new PecanUserData();
            }
*/        }
    }
}
