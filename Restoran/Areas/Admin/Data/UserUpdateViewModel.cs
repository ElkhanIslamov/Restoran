using System.ComponentModel.DataAnnotations;

namespace Restoran.Areas.Admin.Data;

public class UserUpdateViewModel
{
    public required string Fullname { get; set; }
    public required string Username { get; set; }
    [DataType(DataType.EmailAddress)]
    public required string Email { get; set; }
    [DataType(DataType.Password)]
    public string? CurrentPassword { get; set; }
    [DataType(DataType.Password), MinLength(8)]
    public string? NewPassword { get; set; }
    [DataType(DataType.Password), MinLength(8), Compare(nameof(NewPassword))]
    public string? ConfirmNewPassword { get; set; }
}
