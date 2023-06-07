namespace Instagram.Domain.Entity
{
    public class PostImage : IEntity
    {
        public long Id { get; set; }
        public long PostId { get; set; }
        public string File { get; set; }
    }
}
