namespace Instagram.Domain.Entity
{
    public class Post : IEntity
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public long CreatorId { get; set; }
        public string FileName { get; set; }
        public long Created { get; set; }
    }
}
