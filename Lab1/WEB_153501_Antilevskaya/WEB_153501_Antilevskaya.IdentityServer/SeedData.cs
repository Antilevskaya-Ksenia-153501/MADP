using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;
using WEB_153501_Antilevskaya.IdentityServer.Data;
using WEB_153501_Antilevskaya.IdentityServer.Models;

namespace WEB_153501_Antilevskaya.IdentityServer
{
    public class SeedData
    {
        public static async Task EnsureSeedData(WebApplication app)
        {
            using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                context.Database.Migrate();

                var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { "user", "admin" };
                foreach (var role in roles)
                {
                    if (!await roleMgr.RoleExistsAsync(role))
                    {
                        var newRole = new IdentityRole(role);
                        var result = await roleMgr.CreateAsync(newRole);
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                    }
                }
         
                var user = await userMgr.FindByNameAsync("user@gmail.com");
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = "user@gmail.com",
                        Email = "user@gmail.com",
                        EmailConfirmed = true
                    };
                    var result = await userMgr.CreateAsync(user, "Pass123$");
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result =await userMgr.AddToRoleAsync(user, "user");
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = await userMgr.AddClaimsAsync(user, new Claim[] { new Claim(JwtClaimTypes.Name, "Fake User"), new Claim(JwtClaimTypes.GivenName, "User"), new Claim(JwtClaimTypes.FamilyName, "No") });
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    Log.Debug("user created");
                }
                else
                {
                    Log.Debug("user already exists");
                }

                var admin = await userMgr.FindByNameAsync("admin@gmail.com");
                if (admin == null)
                {
                    admin = new ApplicationUser
                    {
                        UserName = "admin@gmail.com",
                        Email = "admin@gmail.com",
                        EmailConfirmed = true
                    };
                    var result = await userMgr.CreateAsync(admin, "Pass123$");
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = await userMgr.AddToRoleAsync(admin, "admin");
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    var claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.Name, "Fake Admin"),
                        new Claim(JwtClaimTypes.GivenName, "Admin"),
                        new Claim(JwtClaimTypes.FamilyName, "No"),
                    };
                    result = await userMgr.AddClaimsAsync(admin, claims);
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    Log.Debug("admin created");
                }
                else
                {
                    Log.Debug("admin already exists");
                }
            }
        }
    }
}