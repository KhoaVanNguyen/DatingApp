namespace DatingApp.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] Hash { get; set; }
        public byte[] Salt { get; set; }
    }
}

