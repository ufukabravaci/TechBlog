using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechBlogApi.Models
{
    public class CommentReaction : ReactionBase
    {
        [Required]
        public int CommentId { get; set; }

        [ForeignKey("CommentId")]
        public Comment Comment { get; set; } = null!;
    }
}