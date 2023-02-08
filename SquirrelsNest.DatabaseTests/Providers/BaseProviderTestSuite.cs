using System;

namespace SquirrelsNest.DatabaseTests.Providers {
    public abstract class BaseProviderTestSuite : IDisposable {
        protected abstract void DeleteDatabase();

        public void Dispose() {
            DeleteDatabase();
        }
    }
}
