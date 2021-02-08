using System.ComponentModel.DataAnnotations;

namespace PhotoMasterBackend.Models
{
    public class User : BaseModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public string Role { get; set; }

        public string Token { get; set; }
    }

    public static class Role
    {
        public const string Admin = "Admin";

        public const string Friend = "Friend";

        public const string Visitor = "Visitor";
    }
}
