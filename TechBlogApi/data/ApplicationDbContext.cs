using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TechBlogApi.Models;

namespace TechBlogApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PostReaction> PostReactions { get; set; }
        public DbSet<CommentReaction> CommentReactions { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserFollow> UserFollows { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserFollow>()
                .HasIndex(uf => new { uf.FollowerId, uf.FollowingId })
                .IsUnique();

            builder.Entity<PostReaction>()
                .HasIndex(pr => new { pr.UserId, pr.PostId })
                .IsUnique(); // A user can only have one reaction per post

            builder.Entity<CommentReaction>()
                .HasIndex(cr => new { cr.UserId, cr.CommentId })
                .IsUnique(); // A user can only have one reaction per comment

            // ApplicationUser - Post Relation (If a user is deleted, their posts are also deleted - Cascade)
            builder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ApplicationUser - Comment relation (If a user is deleted, their comments are also deleted - Cascade)
            builder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Post - Comment relation (If a post is deleted, its comments are also deleted - Cascade)
            builder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            // Comment - ParentComment relation (If a comment is deleted, its sub-comments' ParentCommentId is set to null)
            // So we can still be able to see a deleted comments sub-comments
            builder.Entity<Comment>()
                .HasOne(c => c.ParentComment)
                .WithMany(c => c.SubComments)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.SetNull);

            // ApplicationUser - PostReaction relation (If a user is deleted, their post reactions are also deleted - Cascade)
            builder.Entity<PostReaction>()
                .HasOne(pr => pr.User)
                .WithMany(u => u.PostReactions)
                .HasForeignKey(pr => pr.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserFollow relation (Self-referencing many-to-many relationship - Restrict deletion if following or being followed)
            // To prevent data leaks. We should manually delete the relations(followings in this case) and then we can delete the user
            builder.Entity<UserFollow>()
                .HasOne(uf => uf.Follower)
                .WithMany(u => u.Following)
                .HasForeignKey(uf => uf.FollowerId)
                .OnDelete(DeleteBehavior.Restrict); // Cannot delete a user if they are following others

            builder.Entity<UserFollow>()
                .HasOne(uf => uf.Following)
                .WithMany(u => u.Followers)
                .HasForeignKey(uf => uf.FollowingId)
                .OnDelete(DeleteBehavior.Restrict); // Cannot delete a user if they are being followed

            builder.Entity<Notification>()
                .HasOne(n => n.User) // User who receives the notification
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade); // If a user is deleted, their notifications are also deleted

            builder.Entity<Notification>()
                .HasOne(n => n.RelatedUser)
                .WithMany(u => u.TriggeredNotifications) // A notification is triggered by one user, and a user can trigger many notifications
                .HasForeignKey(n => n.RelatedUserId)
                .OnDelete(DeleteBehavior.SetNull); // If the triggering user is deleted, set the RelatedUserId in the Notification to null

            builder.Entity<Notification>()
                .HasOne(n => n.RelatedPost)
                .WithMany(p => p.PostNotifications) // A notification is related to one post, and a post can have many notifications related to it (likes, comments, etc.)
                .HasForeignKey(n => n.RelatedPostId) 
                .OnDelete(DeleteBehavior.SetNull); // If the related post is deleted, set the RelatedPostId in the Notification to null

            builder.Entity<Notification>()
                .HasOne(n => n.RelatedComment)
                .WithMany(c => c.CommentNotifications) // A notification is related to one comment, and a comment can have many notifications related to it (likes, replies, etc.)
                .HasForeignKey(n => n.RelatedCommentId)
                .OnDelete(DeleteBehavior.SetNull); // If the related comment is deleted, set the RelatedCommentId in the Notification to null
        }
    }
}