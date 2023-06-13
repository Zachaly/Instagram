namespace Instagram.Domain.SqlAttribute
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SqlNameAttribute : Attribute
    {
        public string Name { get; set; } = "";
        public bool OuterQuery { get; set; } = false;
        public bool Conditional { get; set; } = false;

        public SqlNameAttribute(string name)
        {
            Name = name;
        }
        public SqlNameAttribute()
        {

        }
    }
}
