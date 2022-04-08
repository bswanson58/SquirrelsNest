using System;
using LanguageExt.Common;

namespace SquirrelsNest.Service.Dto.Mutations {
    public interface IMutationError {
        string      Message { get; }
        string      Suggestion { get; }
    }

    public class MutationError : IMutationError {
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public  string  Message { get; }
        public  string  Suggestion { get; }

        public MutationError( Error error ) {
            Message = error.Message;
            Suggestion = String.Empty;
        }

        public MutationError( string message ) {
            Message = message;
            Suggestion = String.Empty;
        }

        public MutationError( string message, string suggestion ) {
            Message = message;
            Suggestion = suggestion;
        }
    }
}
