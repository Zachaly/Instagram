namespace Instagram.Domain.Entity
{
    public class UserBlock : IEntity
    {
        public long Id { get; set; }
        public long BlockingUserId { get; set; }
        public long BlockedUserId { get; set; }
    }
}
