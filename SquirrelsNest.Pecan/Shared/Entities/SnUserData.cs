using System;
using System.Text.Json.Serialization;

namespace SquirrelsNest.Pecan.Shared.Entities {
    public class SnUserData : EntityBase {
        public  string      UserId { get; }
        public  string      DataType { get; }
        public  string      Data { get; }

        [JsonConstructor]
        public SnUserData( string entityId, string userId, string dataType, string data ) :
            base( entityId ){
            UserId = EntityIdentifier.CreateIdOrThrow( userId );
            DataType = dataType;
            Data = data;
        }

        public SnUserData( string userId, string dataType, string data ) {
            UserId =  EntityIdentifier.CreateIdOrThrow( userId );
            DataType = dataType;
            Data = data;
        }

        private static SnUserData ? mDefaultData;

        public static SnUserData Default =>
            mDefaultData ??= new SnUserData( EntityIdentifier.Default, EntityIdentifier.Default, String.Empty, String.Empty );
    }
}
