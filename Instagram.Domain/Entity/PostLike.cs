namespace Instagram.Domain.Entity
{
    public class PostLike : IEntity
    {
        public long UserId { get; set; }
        public long PostId { get; set; }
    }
}
