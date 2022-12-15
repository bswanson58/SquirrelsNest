using System;
using System.ComponentModel.DataAnnotations;

namespace SquirrelsNest.Pecan.Server.Database.Entities {
    public class DbEntityBase {
        [Key]
        public string   EntityId { get; set; }

        protected DbEntityBase() {
            EntityId = String.Empty;
        }

        protected DbEntityBase( string entityId ) {
            EntityId = entityId;
        }
    }
}
