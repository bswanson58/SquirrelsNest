using System;
using System.ComponentModel.DataAnnotations;

namespace SquirrelsNest.Pecan.Server.Database.Entities {
    public abstract class DbEntityBase<TEntity> {
        [Key]
        public string   EntityId { get; set; }

        protected DbEntityBase() {
            EntityId = String.Empty;
        }

        protected DbEntityBase( string entityId ) {
            EntityId = entityId;
        }

        public abstract void UpdateFrom( TEntity from );
    }
}
