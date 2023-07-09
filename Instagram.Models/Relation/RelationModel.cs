using Instagram.Domain.SqlAttribute;

namespace Instagram.Models.Relation
{
    public class RelationModel : IModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        [SqlName("[User].[Nickname]")]
        public string UserName { get; set; }
        public string Name { get; set; }
        public IEnumerable<long> ImageIds { get; set; }
    }
}
