using System.ComponentModel.DataAnnotations;

namespace Restoran.Areas.Admin.Data
{
    public class LoginViewModel
    {
        [Required]
        public string UserNameOrEmail { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
