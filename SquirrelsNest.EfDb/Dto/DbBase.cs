﻿using SquirrelsNest.EfDb.Extensions;

namespace SquirrelsNest.EfDb.Dto {
    internal class DbBase {
        public  int         Id { get; set; }
        public  string      EntityId { get; set; }

        protected DbBase() {
            Id = DbIdDefault();
            EntityId = String.Empty;
        }

        public static int DbIdDefault() =>  0;
        public static int DbIdCreate( string from ) => from.ToDbId();
    }
}
