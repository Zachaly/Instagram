using Instagram.Domain.Enum;

namespace Instagram.Models.User.Request
{
    public class UpdateUserRequest
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Nickname { get; set; }
        public string? Bio { get; set; }
        public Gender? Gender { get; set; }
        public string? ProfilePicture { get; set; }
        public string? PasswordHash { get; set; }
    }
}
