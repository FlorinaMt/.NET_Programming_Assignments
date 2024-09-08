using Entities;

namespace RepositoryContracts;

public interface ICommentRepository
{
    Comment AddComment(Comment comment);
    Comment UpdateComment(Comment comment);
    Comment DeleteComment(Comment comment);
    Comment GetCommentById(int id);
    List<Comment> GetCommentsForPost(int postId);
}