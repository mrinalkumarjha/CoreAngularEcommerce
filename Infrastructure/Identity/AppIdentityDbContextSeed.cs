using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if (!userManager.Users.Any())
            {
                var users = new List<AppUser>
                {
                    new AppUser
                    {
                        DisplayName = "mrinal",
                        Email = "mrinalkumarjha@ymail.com",
                        UserName = "mrinalkumarjha@ymail.com",
                        Address = new Address
                        {
                            FirstName = "mrinal",
                            LastName = "jha",
                            Street = "10 The Street",
                            City = "New York",
                            State = "NY",
                            Zipcode = "90210"
                        }
                    },
                    new AppUser
                    {
                        DisplayName = "Admin",
                        Email = "mrinalkumarjha1@gmail.com",
                        UserName = "mrinalkumarjha1@gmail.com"
                    }
                };

                var roles = new List<AppRole>
                {
                    new AppRole {Name = "Admin"},
                    new AppRole {Name = "Member"}
                };

                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Admin@123");
                    await userManager.AddToRoleAsync(user, "Member");
                    if (user.Email == "mrinalkumarjha1@gmail.com") await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}