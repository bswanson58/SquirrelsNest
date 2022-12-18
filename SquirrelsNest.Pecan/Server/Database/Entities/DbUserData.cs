using System;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Server.Database.Entities {
    public class DbUserData : DbEntityBase<DbUserData> {
        public  string          UserId { get; set; }
        public  UserDataType    DataType { get; set; }
        public  string          Data { get; set; }

        public DbUserData() {
            UserId = Shared.Entities.EntityIdentifier.Default;
            DataType = UserDataType.Unknown;
            Data = String.Empty;
        }

        public DbUserData( SnUserData userData ) :
            base( userData.EntityId ) {
            UserId = userData.UserId;
            DataType = userData.DataType;
            Data = userData.Data;
        }

        public static DbUserData From( SnUserData data ) => new DbUserData( data );

        public SnUserData ToEntity() => new SnUserData( EntityId, UserId, DataType, Data );

        public override void Update( DbUserData from ) {
            UserId = from.UserId;
            DataType = from.DataType;
            Data = from.Data;
        }
    }
}
