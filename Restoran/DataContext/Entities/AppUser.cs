using Microsoft.AspNetCore.Identity;

namespace Restoran.DataContext.Entities
{
    public class AppUser:IdentityUser
    {
        public required string Fullname { get; set; }
        public bool IsActive { get; set; }
    }
}
