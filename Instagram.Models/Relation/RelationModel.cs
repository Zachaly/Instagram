using Instagram.Domain.SqlAttribute;

namespace Instagram.Models.Relation
{
    [Join(Table = "User", Condition = "[User].[Id]=[Relation].[UserId]")]
    [Join(Table = "RelationImage", Condition = "[RelationImage].[RelationId]=t.[Id]", OutsideJoin = true)]
    public class RelationModel : IModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        [SqlName("[User].[Nickname]")]
        public string UserName { get; set; }
        public string Name { get; set; }
        [SqlName("[RelationImage].[Id]", OuterQuery = true)]
        public IEnumerable<long> ImageIds { get; set; }
    }
}
