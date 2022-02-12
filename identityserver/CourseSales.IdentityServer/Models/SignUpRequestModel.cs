namespace CourseSales.IdentityServer.Models
{
    public class SignUpRequestModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string City { get; set; }
    }
}
