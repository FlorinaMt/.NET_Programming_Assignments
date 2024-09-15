using Entities;

namespace RepositoryContracts;

public interface ICommentRepository
{
    Task<Comment> AddCommentAsync(Comment comment);
    Task UpdateCommentAsync(Comment comment);
    Task DeleteCommentAsync(int commentId);
    Task<Comment> GetCommentByIdAsync(int id);
    IQueryable<Comment> GetCommentsForPost(int postId);
    IQueryable<Comment> GetAllComments();
}