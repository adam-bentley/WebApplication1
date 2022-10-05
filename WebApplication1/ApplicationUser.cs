namespace WebApplication1
{
    public class ApplicationUser
    {
        public short Id { get; set; }

        public string Username { get; set; }

        public string NormalizedUsername { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Password { get; set; }
    }
}