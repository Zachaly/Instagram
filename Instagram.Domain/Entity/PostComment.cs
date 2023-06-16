namespace Instagram.Domain.Entity
{
    public class PostComment : IEntity
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long PostId { get; set; }
        public string Content { get; set; }
        public long Created { get; set; }
    }
}
