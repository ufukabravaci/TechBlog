using Microsoft.AspNetCore.Identity;
using TechBlogApi.Models;

namespace TechBlogApi.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Sistem rolleri
            var roles = new List<AppRole>
            {
                new AppRole { Name = "Admin", Description = "Sistem yöneticisi", NormalizedName = "ADMIN" },
                new AppRole { Name = "Moderator", Description = "İçerik moderatörü", NormalizedName = "MODERATOR" },
                new AppRole { Name = "User", Description = "Standart kullanıcı", NormalizedName = "USER" }
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role.Name!))
                {
                    await roleManager.CreateAsync(role);
                }
            }

            // Admin kullanıcısı
            var adminUser = new ApplicationUser
            {
                UserName = "admin@techblog.com",
                Email = "admin@techblog.com",
                EmailConfirmed = true,
                FirstName = "Admin",
                LastName = "Ado",
            };

            if (await userManager.FindByEmailAsync(adminUser.Email) == null)
            {
                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRolesAsync(adminUser, new[] { "Admin", "Moderator" });
                }
            }

            // Test kullanıcıları (isteğe bağlı)
            await CreateTestUser(userManager, "moderator@techblog.com", "Moderator123!", "Moderator","Mod","ModLast");
            await CreateTestUser(userManager, "user1@techblog.com", "User123!", "User","User1","UserLastname1");
            await CreateTestUser(userManager, "user2@techblog.com", "User123!", "User", "UserName2", "Userlastname2");
        }

        private static async Task CreateTestUser(
            UserManager<ApplicationUser> userManager,
            string email,
            string password,
            string role,
            string firstName,
            string lastName)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    FirstName = firstName,
                    LastName = lastName
                };
                await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, role);
            }
        }
    }
}