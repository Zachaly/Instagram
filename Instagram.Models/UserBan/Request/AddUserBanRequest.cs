namespace Instagram.Models.UserBan.Request
{
    public class AddUserBanRequest 
    {
        public long UserId { get; set; }
        public long EndDate { get; set; }
    }
}
