using System;
using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Service.Dto {
    public class ClUserData : ClBase {
        public  string          UserId { get; }
        public  UserDataType    DataType { get; }
        public  string          Data { get; }

        public ClUserData( string id, string userId, UserDataType dataType, string data ) :
            base( id ) {
            UserId = userId;
            DataType = dataType;
            Data = data;
        }

        public static ClUserData Default() {
            return new ClUserData( String.Empty, String.Empty, UserDataType.Unknown, String.Empty );
        }
    }

    public static class ClUserDataEx {
        public static ClUserData ToCl( this SnUserData data ) {
            return new ClUserData( data.EntityId, data.UserId, data.DataType, data.Data );
        }
    }
}
