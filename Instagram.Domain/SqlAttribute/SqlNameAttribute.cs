namespace Instagram.Domain.SqlAttribute
{
    public class SqlNameAttribute : Attribute
    {
        public string Name { get; set; }
        public bool OuterQuery { get; set; } = false;

        public SqlNameAttribute(string name)
        {
            Name = name;
        }
    }
}
