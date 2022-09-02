using Microsoft.AspNetCore.Identity;

namespace ParaPharmacie.Models
{
    public class ApplicationUser : IdentityUser
    {
        public  string Name { get; set; }
    }
}
