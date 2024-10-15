namespace StudentApi.Model
{
    public class AuthenticationRequest
    {
        public required string userName { get; set; }
        public required string password { get; set; }
    }
}