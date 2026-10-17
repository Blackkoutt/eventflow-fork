namespace EventFlowAPI.Tests.Infrastructure
{
    public record TestUser(string Id, string Email, string Password);

    public static class TestUsers
    {
        public static readonly TestUser Admin = new("1", "admin@gmail.com", "admin123");
        public static readonly TestUser User2 = new("2", "mateusz.strapczuk2@gmail.com", "789456123qaz");
        public static readonly TestUser User3 = new("3", "mateusz.strapczuk3@gmail.com", "qwe123QWE!@#");
        public static readonly TestUser User4 = new("4", "mateusz.strapczuk4@gmail.com", "qwe123QWE!@#");
        public static readonly TestUser User5 = new("5", "mateusz.strapczuk1@gmail.com", "qazzaq1@WSX");
    }
}
