using System.ComponentModel.DataAnnotations;

namespace SecretsStorage.Models
{
    public abstract class LoginPassword
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
