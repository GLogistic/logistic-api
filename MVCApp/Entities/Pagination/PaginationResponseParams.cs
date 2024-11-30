
namespace Entities.Pagination
{
    public class PaginationResponseParams
    {
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public int totalSize { get; set; }
        public int totalPages { get; set; }
        public Boolean haveNext { get; set; }
        public Boolean havePrev { get; set; }
    }
}
