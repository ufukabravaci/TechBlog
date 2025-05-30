using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechBlogApi.Models
{
    public class PostReaction : ReactionBase
    {
        
        public int? PostId { get; set; }

        [ForeignKey("PostId")]
        public Post Post { get; set; } = null!;
    }
}