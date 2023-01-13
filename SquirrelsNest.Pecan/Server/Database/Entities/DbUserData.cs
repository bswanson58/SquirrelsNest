using System;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Server.Database.Entities {
    public class DbUserData : DbEntityBase<DbUserData> {
        public  string      UserId { get; set; }
        public  string      DataType { get; set; }
        public  string      Data { get; set; }

        public DbUserData() {
            UserId = EntityIdentifier.Default;
            DataType = String.Empty;
            Data = String.Empty;
        }

        public DbUserData( SnUserData userData ) :
            base( userData.EntityId ) {
            UserId = userData.UserId;
            DataType = userData.DataType;
            Data = userData.Data;
        }

        public static DbUserData From( SnUserData data ) => new ( data );

        public SnUserData ToEntity() => new ( EntityId, UserId, DataType, Data );

        public override void UpdateFrom( DbUserData from ) {
            UserId = from.UserId;
            DataType = from.DataType;
            Data = from.Data;
        }
    }
}
