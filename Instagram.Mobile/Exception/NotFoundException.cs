namespace Instagram.Mobile
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string type) : base($"{type} not found")
        {
        }
    }
}
