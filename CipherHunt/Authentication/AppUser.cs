namespace CipherHunt.Authentication
{
    public class AppUser
    {
        public string Token { get; set; }
        public string ID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string AgentCode { get; set; }
        public string AgentBranchCode { get; set; }
        public string BranchCodeChar { get; set; }
    }
    public class CPanelAppUser
    {
        public string ID { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}