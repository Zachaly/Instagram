namespace Instagram.Domain.Entity
{
    public class UserClaim : IEntity
    {
        public long UserId { get; set; }
        public string Value { get; set; }
    }
}
