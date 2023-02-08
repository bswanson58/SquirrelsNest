namespace SquirrelsNest.EfDb.Context {
    public class EfDatabaseConfiguration {
        public  string  ConnectionString { get; set; }

        public EfDatabaseConfiguration() {
            ConnectionString = String.Empty;
        }
    }
}
