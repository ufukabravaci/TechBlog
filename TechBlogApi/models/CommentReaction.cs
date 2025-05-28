using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechBlogApi.Models
{
    public class CommentReaction : ReactionBase
    {
        
        public int? CommentId { get; set; }

        [ForeignKey("CommentId")]
        public Comment Comment { get; set; } = null!;
    }
}