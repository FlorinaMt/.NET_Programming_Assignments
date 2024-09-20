using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class CommentFileRepository:ICommentRepository
{
    private readonly string filePath = "comments.json";

    public CommentFileRepository()
    {
        if(!File.Exists(filePath))
            File.WriteAllText(filePath, "[]");
    }
    
    public async Task<Comment> AddCommentAsync(Comment comment)
    {
        List<Comment> comments = await LoadCommentsAsync();
        int maxId = comments.Count > 0 ? comments.Max(c => c.CommentId) : 1;
        comment.CommentId = maxId + 1;
        comments.Add(comment);
        SaveCommentsAsync(comments);
        return comment;
    }

    public async Task UpdateCommentAsync(Comment comment)
    {
        List<Comment> comments = await LoadCommentsAsync();
        Comment commentToUpdate = await GetCommentByIdAsync(comment.CommentId);
        comments.Remove(commentToUpdate);
        comments.Add(comment);
        SaveCommentsAsync(comments);
    }

    public async Task DeleteCommentAsync(int commentId)
    {
        List<Comment> comments = await LoadCommentsAsync();
        Comment commentToUpdate = await GetCommentByIdAsync(commentId);
        comments.Remove(commentToUpdate);
        SaveCommentsAsync(comments);
    }

    public async Task<Comment> GetCommentByIdAsync(int id)
    {
        List<Comment> comments = await LoadCommentsAsync();
        Comment? comment = comments.SingleOrDefault(c => c.CommentId == id);
        if(comment is null)
            throw new InvalidOperationException("No comment found");
        return comment;
    }

    public IQueryable<Comment> GetCommentsForPost(int postId)
    {
        List<Comment> comments = LoadCommentsAsync().Result;
        
        List<Comment> foundComments =
            comments.FindAll(c => c.PostId == postId);
        if(foundComments.Count==0)
            throw new InvalidOperationException("No comments for this post.");
        return foundComments.AsQueryable();
    }

    public IQueryable<Comment> GetAllComments()
    {
        List<Comment> comments = LoadCommentsAsync().Result;
        
        return comments.AsQueryable();
    }

    private async Task<List<Comment>> LoadCommentsAsync()
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);
        List<Comment> comments =
            JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
        return comments;
    }

    private async void SaveCommentsAsync(List<Comment> toSaveComments)
    {
        string commentsAsJson = JsonSerializer.Serialize(toSaveComments);
        await File.WriteAllTextAsync(filePath, commentsAsJson);
    }
}