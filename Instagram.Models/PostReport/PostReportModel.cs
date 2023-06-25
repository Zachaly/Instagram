using Instagram.Domain.SqlAttribute;

namespace Instagram.Models.PostReport
{
    [Join(Table = "User", Condition = "[User].[Id]=[PostReport].[ReportingUserId]")]
    public class PostReportModel : IModel
    {
        public long Id { get; set; }
        public long ReportingUserId { get; set; }
        [SqlName("[User].[Nickname]")]
        public string ReportingUserName { get; set; }
        public long PostId { get; set; }
        public string Reason { get; set; }
        public long Created { get; set; }
        public bool Resolved { get; set; }
        public long? ResolveTime { get; set; }
        public bool? Accepted { get; set; }
    }
}
