using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechBlogApi.Models;

public abstract class ReactionBase
{
    [Key]
    public int Id { get; set; }
    public bool IsLike { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [Required]
    public string UserId { get; set; } = null!;
    [ForeignKey("UserId")]
    public ApplicationUser User { get; set; } = null!;
}