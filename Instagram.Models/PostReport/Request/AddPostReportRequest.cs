namespace Instagram.Models.PostReport.Request
{
    public class AddPostReportRequest
    {
        public long ReportingUserId { get; set; }
        public string Reason { get; set; }
        public long PostId { get; set; }
    }
}
