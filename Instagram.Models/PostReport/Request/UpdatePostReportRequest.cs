namespace Instagram.Models.PostReport.Request
{
    public class UpdatePostReportRequest
    {
        public long? ReportingUserId { get; set; }
        public long? PostId { get; set; }
        public string? Reason { get; set; }
        public long? Created { get; set; }
        public bool? Resolved { get; set; }
        public long? ResolveTime { get; set; }
        public bool? Accepted { get; set; }
    }
}
