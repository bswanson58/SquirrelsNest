using System.Collections.Generic;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Service.Dto.Mutations {
    // ReSharper disable once ClassNeverInstantiated.Global
    public class UserDataInput {
        public  UserDataType    DataType { get; set; }
        public  string          Data { get; set; }

        public UserDataInput( UserDataType dataType, string data ) {
            DataType = dataType;
            Data = data;
        }
    }

    public class UserDataPayload {
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public  ClUserData ?        UserData { get; }
        public  List<MutationError> Errors { get; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global

        public UserDataPayload( ClUserData data ) {
            Errors = new List<MutationError>();
            UserData = data;
        }

        public UserDataPayload( Error error ) {
            Errors = new List<MutationError>{ new MutationError( error ) };
            UserData = ClUserData.Default();
        }

        public UserDataPayload( string error ) {
            Errors = new List<MutationError> { new MutationError( error ) };
            UserData = ClUserData.Default();
        }
    }
}
