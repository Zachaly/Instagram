namespace Instagram.Models.VerificationRequest.Request
{
    public class AddAccountVerificationRequest
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
