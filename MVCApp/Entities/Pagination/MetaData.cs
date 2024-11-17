namespace Entities.Pagination
{
    public class MetaData
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalSize { get; set; }

        public bool HaveNext => CurrentPage < TotalPages;
        public bool HavePrev => CurrentPage > 1;
    }
}
