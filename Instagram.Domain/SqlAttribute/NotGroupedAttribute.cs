namespace Instagram.Domain.SqlAttribute
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class NotGroupedAttribute : Attribute
    {
    }
}
