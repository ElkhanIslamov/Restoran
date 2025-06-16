using System.ComponentModel.DataAnnotations;

namespace Restoran.Areas.Admin.Data
{
    public class RegisterViewModel
    {
        [Required]
        public string Fullname { get; set; }
        [Required]
        public string Username { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password), Compare(nameof(Password))]
        public string PasswordConfirm { get; set; }
    }
}
