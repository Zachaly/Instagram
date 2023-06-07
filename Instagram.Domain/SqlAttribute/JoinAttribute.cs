namespace Instagram.Domain.SqlAttribute
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class JoinAttribute : Attribute
    {
        public string Table { get; set; }
        public string Condition { get; set; }
        public bool OutsideJoin { get; set; } = false;
    }
}
