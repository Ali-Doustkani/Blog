using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Account
{
    public class LoginEntry
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
