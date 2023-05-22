using Instagram.Domain.Enum;

namespace Instagram.Models.User.Request
{
    public class RegisterRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
        public Gender Gender { get; set; }
    }
}
