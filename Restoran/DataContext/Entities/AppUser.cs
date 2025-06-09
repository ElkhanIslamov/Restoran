using Microsoft.AspNetCore.Identity;

namespace Restoran.DataContext.Entities
{
    public class AppUser:IdentityUser
    {
        public required string FullName { get; set; }
    }
}
