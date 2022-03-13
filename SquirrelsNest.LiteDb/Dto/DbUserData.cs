using LiteDB;
using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.LiteDb.Dto {
    internal class DbUserData : DbBase {
        public  string          UserId { get; set; }
        public  UserDataType    DataType { get; set; }
        public  string          Data { get; set; }

        protected DbUserData() {
            UserId = Common.Values.EntityId.Default;
            DataType = UserDataType.Unknown;
            Data = String.Empty;
        }

        public static DbUserData From( SnUserData data ) {
            return new DbUserData {
                Id = String.IsNullOrWhiteSpace( data.DbId ) ? ObjectId.NewObjectId() : new ObjectId( data.DbId ),
                EntityId = data.EntityId,
                UserId = data.UserId,
                DataType = data.DataType,
                Data = data.Data
            };
        }

        public SnUserData ToEntity() {
            return new SnUserData( EntityId, Id.ToString(), UserId, DataType, Data );
        }
    }
}
