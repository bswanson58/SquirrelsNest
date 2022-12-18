using System;

namespace SquirrelsNest.Pecan.Shared.Entities {
    public enum UserDataType {
        Unknown = 0,
        LastProject = 1,
        IssueListFormat = 2,
        AlmondClient = 3
    }

    public class SnUserData : EntityBase {
        public  EntityIdentifier    UserId { get; }
        public  UserDataType        DataType { get; }
        public  string              Data { get; }

        public SnUserData( string entityId, string userId, UserDataType dataType, string data ) :
            base( entityId ){
            UserId = EntityIdentifier.CreateIdOrThrow( userId );
            DataType = dataType;
            Data = data;
        }

        public SnUserData( EntityIdentifier userId, UserDataType dataType, string data ) :
            base( String.Empty ) {
            UserId = userId;
            DataType = dataType;
            Data = data;
        }

        private static SnUserData ? mDefaultData;

        public static SnUserData Default =>
            mDefaultData ??= new SnUserData( EntityIdentifier.Default, EntityIdentifier.Default, UserDataType.Unknown, String.Empty );
    }
}
