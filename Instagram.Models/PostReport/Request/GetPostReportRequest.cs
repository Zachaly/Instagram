namespace Instagram.Models.PostReport.Request
{
    public class GetPostReportRequest : PagedRequest
    {
        public long? Id { get; set; }
        public long? ReportingUserId { get; set; }
        public long? PostId { get; set; }
        public string? Reason { get; set; }
        public long? Created { get; set; }
        public bool? Resolved { get; set; }
        public long? ResolveTime { get; set; }
    }
}
