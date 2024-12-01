
namespace Entities.Pagination
{
    public class PaginationResponseParams
    {
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public int totalSize { get; set; }
        public int totalPages { get; set; }
        public bool haveNext { get; set; }
        public bool havePrev { get; set; }
    }
}
