using System;

namespace SquirrelsNest.Pecan.Shared.Dto {
    public class BaseResponse {
        public  bool    Succeeded { get; }
        public  string  Message { get; }

        protected BaseResponse() {
            Succeeded = true;
            Message = String.Empty;
        }

        protected BaseResponse( Exception ex ) {
            Succeeded = false;
            Message = ex.Message;
        }
    }
}