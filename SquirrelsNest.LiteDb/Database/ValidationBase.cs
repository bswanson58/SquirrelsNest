using System.ComponentModel.DataAnnotations;
using LanguageExt;
using LanguageExt.Common;
using LiteDB;
using SquirrelsNest.LiteDb.Dto;

namespace SquirrelsNest.LiteDb.Database {
    internal class ValidationBase<T> where T : DbBase {
        protected Either<Error, T> ValidateEntity( T ? entity ) {
            if( entity == null ) {
                return Error.New( new ValidationException( "Entity cannot be null" ));
            }

            return entity;
        }

        protected Either<Error, ObjectId> ValidateObjectId( ObjectId ? id ) {
            if( id == null ) {
                return Error.New( new ValidationException( "Object id is null" ));
            }

            if( id.Equals( new ObjectId())) {
                return Error.New( new ValidationException( "ObjectId is not initialized" ));
            }

            return id;
        }

        protected Either<Error, Action<IEnumerable<T>>> ValidateAction( Action<IEnumerable<T>> ? action ) {
            if( action == null ) {
                return Error.New( new ValidationException( "Action cannot be null" ));
            }

            return action;
        }

        protected Either<Error, Action<ILiteQueryable<T>>> ValidateAction( Action<ILiteQueryable<T>> ? action ) {
            if( action == null ) {
                return Error.New( new ValidationException( "Action cannot be null" ));
            }

            return action;
        }

        protected Either<Error, Action<ILiteCollection<T>>> ValidateAction( Action<ILiteCollection<T>> ? action ) {
            if( action == null ) {
                return Error.New( new ValidationException( "Action cannot be null" ));
            }

            return action;
        }

        protected Either<Error, string> ValidateString( string value ) {
            if( String.IsNullOrWhiteSpace( value )) {
                return Error.New( new ValidationException( "String value cannot be empty or null" ));
            }

            return value;
        }
    }
}
