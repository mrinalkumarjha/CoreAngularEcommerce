using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    //bob@test.com
                    DisplayName = "Mrinal",
                    Email = "mrinalkumarjha@ymail.com",
                    UserName = "mrinalkumarjha@ymail.com",
                    Address = new Address
                    {
                        FirstName = "mrinal",
                        LastName = "jha",
                        Street = "hindon vihar",
                        City = "Noida",
                        State = "UP",
                        Zipcode = "222558"
                    }
                };

                await userManager.CreateAsync(user, "Admin@123");
            }
        }
    }
}