using Microsoft.AspNetCore.Identity;

namespace Ajerly_Platform.Models
{
    public class ApplicationUser : IdentityUser
    {
        // allow null so default Identity register works without providing FullName
        public string? FullName { get; set; }
    }
}
