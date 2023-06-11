namespace Instagram.Domain.SqlAttribute
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class WhereAttribute : Attribute
    {
        public string Condition { get; set; }
    }
}
