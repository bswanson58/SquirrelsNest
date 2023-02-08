using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.EfDb.Dto {
    internal class DbUserData : DbBase {
        public  string          UserId { get; set; }
        public  UserDataType    DataType { get; set; }
        public  string          Data { get; set; }

        protected DbUserData() {
            UserId = String.Empty;
            DataType = UserDataType.Unknown;
            Data = String.Empty;
        }

        public static DbUserData From( SnUserData data ) {
            return new DbUserData {
                EntityId = data.EntityId,
                Id = String.IsNullOrWhiteSpace( data.DbId ) ? DbIdDefault() : DbIdCreate( data.DbId ),
                UserId = data.UserId,
                DataType = data.DataType,
                Data = data.Data
            };
        }

        public SnUserData ToEntity() {
            return new SnUserData( EntityId, Id.ToString(), Common.Values.EntityId.CreateIdOrThrow( UserId ), DataType, Data );
        }
    }
}
