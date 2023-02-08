using System;

namespace SquirrelsNest.Pecan.Server.Features.Transfer.Dto {
    internal class TrBase {
        public  string      EntityId { get; set; }

        protected TrBase() {
            EntityId = String.Empty;
        }
    }
}
