﻿using LiteDB;

namespace SquirrelsNest.LiteDb.Dto {
    internal class DbUser : DbBase {
        protected DbUser( ObjectId id )
            : base( id ) { }
    }
}
