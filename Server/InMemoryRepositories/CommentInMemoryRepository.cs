using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class CommentInMemoryRepository : ICommentRepository
{
    private List<Comment> comments;

    public CommentInMemoryRepository()
    {
        comments = new List<Comment>();
        comments.Add(new Comment{CommentBody = "I won't miss it. Tak for det.", PostId = 1, UserId = 2});
        comments.Add(new Comment{CommentBody = "I have DNP at that time, I won't see it, oh nej.", PostId = 1, UserId = 3});
        comments.Add(new Comment{CommentBody = "DNP is interesting and I'm not saying this because other people are reading my comment. ", PostId = 2, UserId = 5});
        comments.Add(new Comment{CommentBody = "I made you read my comment haha.", PostId = 3, UserId = 4});
        comments.Add(new Comment{CommentBody = "Do you guys have a dad?", PostId = 4, UserId = 5});
        comments.Add(new Comment{CommentBody = "I have 2 dads.", PostId = 4, UserId = 2});
        comments.Add(new Comment{CommentBody = "Who cares?", PostId = 5, UserId = 4});
    }
    
    public Task<Comment> AddCommentAsync(Comment comment)
    {
        comment.CommentId =
            comments.Any() ? comments.Max(c => c.CommentId) + 1 : 1;
        comments.Add(comment);
        return Task.FromResult(comment);
    }

    public Task UpdateCommentAsync(Comment comment)
    {
        /*Comment? commentToUpdate = comments.SingleOrDefault(c => c.CommentId == comment.CommentId);
        if (commentToUpdate is null)
            throw new InvalidOperationException("No comment found");*/
        Comment commentToUpdate = GetCommentByIdAsync(comment.CommentId).Result;
        comments.Remove(commentToUpdate);
        comments.Add(comment);
        return Task.CompletedTask;
    }

    public Task DeleteCommentAsync(int commentId)
    {
        Comment commentToDelete = GetCommentByIdAsync(commentId).Result;
        comments.Remove(commentToDelete);
        return Task.CompletedTask;
    }

    public Task<Comment> GetCommentByIdAsync(int id)
    {
        Comment? foundComment =
            comments.SingleOrDefault(c => c.CommentId == id);
        if(foundComment is null)
            throw new InvalidOperationException("No comment found");
        return Task.FromResult(foundComment);
    }

    public IQueryable<Comment> GetCommentsForPost(int postId)
    {
        List<Comment> foundComments =
            comments.FindAll(c => c.PostId == postId);
        if(foundComments.Count==0)
            throw new InvalidOperationException("No comments for this post.");
        return foundComments.AsQueryable();
    }

    public IQueryable<Comment> GetAllComments()
    {
        return comments.AsQueryable();
    }

    public string ToString()
    {
        string s = "";
        for(int i=0; i<comments.Count; i++)
            s=s+comments[i].ToString()+'\n';
        return s;
    }
}