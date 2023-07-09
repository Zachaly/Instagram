namespace Instagram.Domain.Entity
{
    public class Relation : IEntity
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; }
    }
}
