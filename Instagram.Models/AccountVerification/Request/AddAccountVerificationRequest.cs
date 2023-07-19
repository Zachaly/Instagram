namespace Instagram.Models.AccountVerification.Request
{
    public class AddAccountVerificationRequest
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
    }
}
