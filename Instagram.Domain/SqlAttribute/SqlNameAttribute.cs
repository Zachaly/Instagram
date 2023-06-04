namespace Instagram.Domain.SqlAttribute
{
    public class SqlNameAttribute : Attribute
    {
        public string Name { get; set; }

        public SqlNameAttribute(string name)
        {
            Name = name;
        }
    }
}
