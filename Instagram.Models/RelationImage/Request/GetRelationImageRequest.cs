namespace Instagram.Models.RelationImage.Request
{
    public class GetRelationImageRequest : PagedRequest
    {
        public long? Id { get; set; }
        public long? RelationId { get; set; }
    }
}
