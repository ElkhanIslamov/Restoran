using System.ComponentModel.DataAnnotations;

namespace Restoran.Areas.Admin.Data
{
    public class ForgotPasswordViewModel
    {
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
    }
}
