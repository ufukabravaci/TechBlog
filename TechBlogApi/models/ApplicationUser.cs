using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace TechBlogApi.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Location { get; set; }
    public string? Bio { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime? BirthDate { get; set; }
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    public int Score { get; set; } = 0;

    //Navigation Properties
    public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; } = null!;

    public ICollection<Post> Posts = new List<Post>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<PostReaction> PostReactions { get; set; } = new List<PostReaction>();
    public ICollection<CommentReaction> CommentReactions { get; set; } = new List<CommentReaction>();

    public ICollection<Notification> TriggeredNotifications { get; set; } = new List<Notification>(); //We can track our actions, may be unnescessery

    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public ICollection<UserFollow> Followers { get; set; } = new List<UserFollow>();
    public ICollection<UserFollow> Following { get; set; } = new List<UserFollow>();
}