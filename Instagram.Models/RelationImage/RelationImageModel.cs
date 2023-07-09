namespace Instagram.Models.RelationImage
{
    public class RelationImageModel : IModel
    {
        public long Id { get; set; }
        public long RelationId { get; set; }
        public string FileName { get; set; }
    }
}
