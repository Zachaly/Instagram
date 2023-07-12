namespace Instagram.Domain.Entity
{
    public class RelationImage : IEntity
    {
        public long Id { get; set; }
        public long RelationId { get; set; }
        public string FileName { get; set; }
    }
}
