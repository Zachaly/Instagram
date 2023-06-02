namespace Instagram.Models
{
    public abstract class PagedRequest
    {
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public bool? SkipPagination { get; set; }
    }
}
