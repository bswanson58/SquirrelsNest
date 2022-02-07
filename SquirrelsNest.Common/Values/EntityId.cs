using LanguageExt;

namespace SquirrelsNest.Common.Values {
    public class EntityId {
        public  string  Value { get; }

        public static implicit operator string( EntityId issueId ) => issueId.Value;

        private EntityId( string value ) {
            if(!IsValid( value )) {
                throw new ArgumentNullException( value );
            }

            Value = value;
        }

        private static bool IsValid( string value ) {
            return !String.IsNullOrWhiteSpace( value );
        }

        public static Option<EntityId> For( string value ) => 
            IsValid( value ) ? Option<EntityId>.Some( new EntityId( value )) : 
                Option<EntityId>.None;

        internal static EntityId Default => new EntityId( "default" );
    }
}
