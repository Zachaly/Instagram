namespace Instagram.Domain.Entity
{
    public class PostReport : IEntity
    {
        public long Id { get; set; }
        public long ReportingUserId { get; set; }
        public long PostId { get; set; }
        public string Reason { get; set; }
        public long Created { get; set; }
        public bool? Accepted { get; set; }
        public bool Resolved { get; set; }
        public long? ResolveTime { get; set; }
    }
}
