using System.Diagnostics;
using LanguageExt;

namespace SquirrelsNest.Common.Values {
    [DebuggerDisplay("Id:{" + nameof( Value ) + "}")]
    public class EntityId : IEquatable<EntityId> {
        internal static EntityId Default => new ( "default" );

        public string Value { get; }

        public static implicit operator string( EntityId issueId ) => issueId.Value;

        private EntityId( string value ) {
            if( !IsValid( value ) ) {
                throw new ArgumentNullException( value );
            }

            Value = value;
        }

        private static bool IsValid( string value ) {
            return !String.IsNullOrWhiteSpace( value );
        }

        public static Option<EntityId> For( string value ) =>
            IsValid( value ) ? Option<EntityId>.Some( new EntityId( value ) ) :
                Option<EntityId>.None;

        // Equality:
        public override bool Equals( object? obj ) => Equals( obj as EntityId );

        public bool Equals( EntityId? right ) {
            if( right is null ) {
                return false;
            }

            // Optimization for a common success case.
            if( ReferenceEquals( this, right )) {
                return true;
            }

            // If run-time types are not exactly the same, return false.
            if( GetType() != right.GetType()) {
                return false;
            }

            // Return true if the fields match.
            // Note that the base class is not invoked because it is
            // System.Object, which defines Equals as reference equality.
            return Value == right.Value;
        }

        public override int GetHashCode() => Value.GetHashCode();

        public static bool operator == ( EntityId ? lhs, EntityId ? rhs ) {
            if( lhs is null ) {
                if( rhs is null ) {
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return lhs.Equals( rhs );
        }

        public static bool operator !=( EntityId lhs, EntityId rhs ) => !( lhs == rhs );
    }
}
