using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechBlogApi.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Message { get; set; } = null!;

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public NotificationType Type { get; set; }

        // User who triggered notification(liked the post , followed etc)
        // Nullable because may be some system notification may be added to app
        public string? RelatedUserId { get; set; }
        [ForeignKey("RelatedUserId")]
        public ApplicationUser? RelatedUser { get; set; } 

        // Which post caused the notification. nullable
        public int? RelatedPostId { get; set; }
        [ForeignKey("RelatedPostId")]
        public Post? RelatedPost { get; set; } 

        // Notification related comment. nullable
        public int? RelatedCommentId { get; set; }
        [ForeignKey("RelatedCommentId")]
        public Comment? RelatedComment { get; set; } 

        //Who will get the notification. Required
        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = null!; 
    }

    public enum NotificationType
    {
        Follow,
        PostLike,
        PostDislike,
        CommentLike,
        CommentDislike,
        NewComment,
        NewReply
    }
}