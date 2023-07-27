namespace Instagram.Domain.Entity
{
    public class UserStoryImage : IEntity
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long Created { get; set; }
        public string FileName { get; set; }
    }
}
