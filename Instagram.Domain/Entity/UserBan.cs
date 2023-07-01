namespace Instagram.Domain.Entity
{
    public class UserBan : IEntity
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long StartDate { get; set; }
        public long EndDate { get; set; }
    }
}
