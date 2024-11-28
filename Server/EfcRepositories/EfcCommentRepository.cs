using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcCommentRepository : ICommentRepository
{
    private readonly AppContext context;

    public EfcCommentRepository(AppContext context)
    {
        this.context = context;
    }

    public async Task<Comment> AddCommentAsync(Comment comment)
    {
        EntityEntry<Comment> entityEntry =
            await context.Comments.AddAsync(comment);
        await context.SaveChangesAsync();
        return entityEntry.Entity;
    }

    public async Task UpdateCommentAsync(Comment comment)
    {
        if (!await context.Comments.AnyAsync(c =>
                c.CommentId == comment.CommentId))
            throw new ArgumentException(
                $"Comment with ID {comment.CommentId} not found");
        context.Comments.Update(comment);
        await context.SaveChangesAsync();
    }

    public async Task DeleteCommentAsync(int commentId)
    {
        Comment? commentToBeDeleted =
            await context.Comments.SingleOrDefaultAsync(c =>
                c.CommentId == commentId);
        if (commentToBeDeleted is null)
            throw new ArgumentException(
                $"Comment with ID {commentId} not found");

        context.Comments.Remove(commentToBeDeleted);
        await context.SaveChangesAsync();
    }

    public async Task<Comment> GetCommentByIdAsync(int commentId)
    {
        Comment? comment =
            await context.Comments.Include(c => c.Post).Include(c => c.User)
                .SingleOrDefaultAsync(c =>
                    c.CommentId == commentId);
        if (comment is null)
            throw new ArgumentException(
                $"Comment with ID {comment} not found");
        return comment;
    }

    public IQueryable<Comment> GetCommentsForPost(int postId)
    {
        return context.Comments.Include(c => c.Post).Include(c => c.User)
            .Where(c => c.Post.PostId == postId)
            .AsQueryable();
    }

    public IQueryable<Comment> GetAllComments()
    {
        return context.Comments.Include(c => c.Post).Include(c => c.User)
            .AsQueryable();
    }
}