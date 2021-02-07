using Microsoft.AspNetCore.Identity;

namespace Core.Entities.Identity
{
    // We installed package Microsoft.Extensions.Identity.Stores in core project because app user need to extend from IdentityUser
    public class AppUser: IdentityUser
    {
        public string DisplayName { get; set; }
        public Address Address { get; set; }
        
    }
}