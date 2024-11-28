using System.Xml.Serialization;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcPostRepository : IPostRepository
{
    private readonly AppContext context;

    public EfcPostRepository(AppContext context)
    {
        this.context = context;
    }

    public async Task<Post> AddPostAsync(Post post)
    {
        EntityEntry<Post> entityEntry = await context.Posts.AddAsync(post);
        await context.SaveChangesAsync();
        return entityEntry.Entity;
    }

    public async Task UpdatePostAsync(Post post)
    {
        if (!await context.Posts.AnyAsync(p => p.PostId == post.PostId))
            throw new ArgumentException(
                $"Post with ID {post.PostId} not found");
        context.Posts.Update(post);
        await context.SaveChangesAsync();
    }

    public async Task DeletePostAsync(int postId)
    {
        Post? postToBeDeleted =
            await context.Posts.SingleOrDefaultAsync(p => p.PostId == postId);
        if (postToBeDeleted is null)
            throw new ArgumentException(
                $"Post with ID {postId} not found");

        context.Posts.Remove(postToBeDeleted);
        await context.SaveChangesAsync();
    }

    public async Task<Post> GetPostByIdAsync(int postId)
    {
        Post? post = await context.Posts.Include(p => p.User)
            .Include(p => p.Comments).Include(p => p.Likes)
            .SingleOrDefaultAsync(p => p.PostId == postId);
        if (post is null)
            throw new ArgumentException(
                $"Post with ID {postId} not found");
        return post;
    }

    public IQueryable<Post> GetPosts()
    {
        return context.Posts.Include(p => p.User).Include(p => p.Likes)
            .Include(p => p.Comments).AsQueryable();
    }
}