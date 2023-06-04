namespace Instagram.Domain.SqlAttribute
{
    public class JoinAttribute : Attribute
    {
        public string Table { get; set; }
        public string Condition { get; set; }
    }
}
