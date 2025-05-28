using Microsoft.AspNetCore.Identity;

namespace TechBlogApi.Models
{
    public class AppRole : IdentityRole
    {
        // Ek özellikler eklenebilir
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}