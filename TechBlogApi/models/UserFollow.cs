using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechBlogApi.Models
{
    public class UserFollow
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public string FollowerId { get; set; } = null!;
        [ForeignKey("FollowerId")]
        public ApplicationUser? Follower { get; set; }
        
        public string FollowingId { get; set; } = null!;
        [ForeignKey("FollowingId")]
        public ApplicationUser? Following { get; set; }
    }
}