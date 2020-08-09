namespace AspCore.ApiClient.Entities
{
    public class ApiAuthentication
    {
        public bool Enabled { get; set; }
        public string BaseAddress { get; set; }
        public string TokenPath { get; set; }
        public string RefreshTokenPath { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
