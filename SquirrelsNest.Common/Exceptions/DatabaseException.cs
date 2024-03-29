﻿namespace SquirrelsNest.Common.Exceptions {
    public class DatabaseException : ApplicationException {
        public DatabaseException( string message ) :
            base( message ) { }

        public DatabaseException( string message, Exception innerException ) :
            base( message, innerException ) { }
    }
}
