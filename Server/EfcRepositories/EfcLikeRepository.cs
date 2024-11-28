using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcLikeRepository : ILikeRepository
{
    private readonly AppContext context;

    public EfcLikeRepository(AppContext context)
    {
        this.context = context;
    }

    public async Task<Like> AddLikeAsync(Like like)
    {
        EntityEntry<Like> entityEntry = await context.Likes.AddAsync(like);
        await context.SaveChangesAsync();
        return entityEntry.Entity;
    }

    public async Task DeleteLikeAsync(int likeId)
    {
        Like? likeToBeDeleted =
            await context.Likes.SingleOrDefaultAsync(l => l.LikeId == likeId);
        if (likeToBeDeleted is null)
            throw new ArgumentException(
                $"Like with ID {likeId} not found");

        context.Likes.Remove(likeToBeDeleted);
        await context.SaveChangesAsync();
    }

    public async Task<Like> GetLikeByIdAsync(int id)
    {
        Like? like =
            await context.Likes.Include(l => l.Post).Include(l => l.User)
                .SingleOrDefaultAsync(l => l.LikeId == id);
        if (like is null)
            throw new ArgumentException($"Like with ID {id} not found");
        return like;
    }

    public IQueryable<Like> GetLikesForPost(int postId)
    {
        return context.Likes.Include(l => l.Post).Include(l => l.User)
            .Where(l => l.Post.PostId == postId).AsQueryable();
    }

    public IQueryable<Like> GetAllLikes()
    {
        return context.Likes.Include(l => l.Post).Include(l => l.User)
            .AsQueryable();
    }
}