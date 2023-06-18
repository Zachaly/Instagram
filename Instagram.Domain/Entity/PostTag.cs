namespace Instagram.Domain.Entity
{
    public class PostTag : IEntity
    {
        public long PostId { get; set; }
        public string Tag { get; set; }
    }
}
