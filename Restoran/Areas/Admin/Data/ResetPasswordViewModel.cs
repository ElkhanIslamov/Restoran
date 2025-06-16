using System.ComponentModel.DataAnnotations;

namespace Restoran.Areas.Admin.Data
{
    public class ResetPasswordViewModel
    {
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }
        public required string Token { get; set; }
    }
}
