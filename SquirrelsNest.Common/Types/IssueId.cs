using LanguageExt;

namespace SquirrelsNest.Common.Types {
    public class IssueId {
        public  string  Value { get; }

        public static implicit operator string( IssueId issueId ) => issueId.Value;

        private IssueId( string value ) {
            if(!IsValid( value )) {
                throw new ArgumentNullException( value );
            }

            Value = value;
        }

        private static bool IsValid( string value ) {
            return !String.IsNullOrWhiteSpace( value );
        }

        public static Option<IssueId> For( string value ) => 
            IsValid( value ) ? Option<IssueId>.Some( new IssueId( value )) : 
                               Option<IssueId>.None;
    }
}
