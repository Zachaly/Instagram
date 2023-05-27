namespace Instagram.Models.User
{
    public class LoginResponse
    {
        public long UserId { get; set; }
        public string Email { get; set; }
        public string AuthToken { get; set; }
    }
}
