namespace Teatro.Models.Dto.Responses
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Cognome { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>();
    }
}