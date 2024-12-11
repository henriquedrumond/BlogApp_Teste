using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;

namespace BlogApp
{
    public class PostService
    {
        private readonly BlogDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public PostService(BlogDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task<Post> CreatePostAsync(int userId, string title, string content)
        {
            var post = new Post { UserId = userId, Title = title, Content = content, CreatedAt = DateTime.Now };
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "New post created!");
            return post;
        }

        public async Task<Post> EditPostAsync(int userId, int postId, string title, string content)
        {
            var post = await _context.Posts.FindAsync(postId);
            if (post == null || post.UserId != userId)
                throw new UnauthorizedAccessException();

            post.Title = title;
            post.Content = content;
            post.CreatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return post;
        }

        public async Task<bool> DeletePostAsync(int userId, int postId)
        {
            var post = await _context.Posts.FindAsync(postId);
            if (post == null || post.UserId != userId)
                throw new UnauthorizedAccessException();

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Post>> GetPostsAsync()
        {
            return await _context.Posts.ToListAsync();
        }
    }
}