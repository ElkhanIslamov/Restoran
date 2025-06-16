using System.ComponentModel.DataAnnotations;

namespace Restoran.Areas.Admin.Data
{
    public class SubmitResetPasswordViewModel
    {
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
