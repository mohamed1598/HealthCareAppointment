namespace Auth.Dtos
{
    public class LoginResponse(string email, string token, string[] roles)
    {
        public string Email { get; set; } = email;
        public string Token { get; set; } = token;
        public string[] Roles { get; set; } = roles;

    }
}
