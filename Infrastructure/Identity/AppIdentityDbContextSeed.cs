using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if(userManager.Users.Any())
            {
                var user = new AppUser {
                    DisplayName = "Mrinal",
                    Email= "mrinalkumarjha@ymail.com",
                    UserName = "mrinalkumarjha@ymail.com",
                    PhoneNumber = "9015151449",
                    Address = new Address{
                        FirstName = "Mrinal",
                        LastName = "jha",
                        Street = "noida",
                        City = "noida",
                        State = "Delhi",
                        Zipcode = "110092"
                    }

                };

                await userManager.CreateAsync(user, "Mrinal@124");
            }
        }
    }
}