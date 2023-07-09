namespace Instagram.Models.Relation.Request
{
    public class GetRelationRequest : PagedRequest
    {
        public long? Id { get; set; }
        public long? UserId { get; set; }
    }
}
