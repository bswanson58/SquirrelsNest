namespace SquirrelsNest.Service.Controllers.Dto {
    public class PageInfo {
        private int             mRecordsPerPage = 10;
        private readonly int    mMaxRecordsPerPage = 50;

        public int  Page { get; set; } = 1;

        public int RecordsPerPage {
            get => mRecordsPerPage;
            set => mRecordsPerPage = ( value > mMaxRecordsPerPage ) ? mMaxRecordsPerPage : value;
        }
    }
}
