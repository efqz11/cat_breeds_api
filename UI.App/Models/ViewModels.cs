using System.ComponentModel.DataAnnotations;

namespace UI.App.Models
{
    public class LoginVm
    {
        [EmailAddress]
        [Required]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string? ReturnUrl { get; set; }
        public bool RememberMe { get; set; } = false;
    }
}
