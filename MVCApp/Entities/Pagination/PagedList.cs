namespace Entities.Pagination
{
    public class PagedList<T> : List<T>
    {
        public MetaData MetaData { get; set; }

        public PagedList(List<T> items, int totalSize, int currentPage, int pageSize)
        {
            MetaData = new MetaData
            {
                CurrentPage = currentPage,
                TotalPages = (int)Math.Ceiling(totalSize / (double)pageSize),
                PageSize = pageSize,
                TotalSize = totalSize,
            };

            AddRange(items);
        }
    }
}
