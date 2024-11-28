using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class CommentFileRepository : ICommentRepository
{
    private readonly string filePath = "comments.json";

    public CommentFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
            AddDummyDataAsync();
        }
        
    }
    
    public async Task AddDummyDataAsync()
    {
        int delay = 0;
        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
        {
            CommentBody = "I won't miss it. Tak for det.", PostId = 1,
            UserId = 2
        });
        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
        {
            CommentBody =
                "I have DNP at that time, I won't see it, oh nej.",
            PostId = 1, UserId = 3
        });
        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
        {
            CommentBody = "Remember, the Earth is flat.", PostId = 1,
            UserId = 2
        });


        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
        {
            CommentBody = "Do you guys have a dad?", PostId = 2, UserId = 5
        });
        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
            { CommentBody = "I have 2 dads.", PostId = 2, UserId = 2 });
        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
        {
            CommentBody =
                "What did one plate whisper to the other plate? A: Dinner is on me.",
            PostId = 2, UserId = 1
        });
        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
        {
            CommentBody =
                "What do you call cheese that’s not your cheese? A: Nacho cheese.",
            PostId = 2, UserId = 3
        });

        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
        {
            CommentBody = "I made you read my comment haha.", PostId = 3,
            UserId = 4
        });

        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
        {
            CommentBody =
                "Kevin Dutton - The good psychopath's guide to success.",
            PostId = 4, UserId = 4
        });
        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
        {
            CommentBody = "Giorgio Faletti - I kill.", PostId = 4,
            UserId = 2
        });
        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
        {
            CommentBody =
                "I dont read, I'm to smart for that. Do smth useful with you're time. The writer's just wanna sell they're books, but I dont put my money their.",
            PostId = 4, UserId = 2
        });

        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
        {
            CommentBody =
                "DNP is interesting and I'm not saying this because other people are reading my comment. ",
            PostId = 5, UserId = 3
        });
        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
            { CommentBody = "Aah nej! ", PostId = 5, UserId = 2 });

        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
            { CommentBody = "Who cares?", PostId = 6, UserId = 4 });

        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
        {
            CommentBody =
                "Put aluminium foil in the microwave, turn it on for 2 minutes, take it out and smell it. Works 100%",
            PostId = 7, UserId = 1
        });
        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
        {
            CommentBody =
                "Yes, the solution @betelgeuse suggested is great.",
            PostId = 7, UserId = 2
        });
        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
        {
            CommentBody =
                "I was skeptical about it, but it just took my pain away. Amazing!",
            PostId = 7, UserId = 4
        });
        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
        {
            CommentBody =
                "I can't believe it works! It works faster if you use more aluminium foil.",
            PostId = 7, UserId = 3
        });

        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
        {
            CommentBody =
                "Water might not be wet. This is because most scientists define wetness as a liquid’s ability to maintain contact with a solid surface, meaning that water itself is not wet, but can make other objects wet.",
            PostId = 8, UserId = 4
        });
    }

    public async Task<Comment> AddCommentAsync(Comment comment)
    {
        List<Comment> comments = await LoadCommentsAsync();
        int maxId = comments.Count > 0 ? comments.Max(c => c.CommentId) : 0;
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
        Comment commentToDelete = await GetCommentByIdAsync(commentId);
        foreach(Comment comment in comments.ToList())
        {
            if (comment.CommentId == commentToDelete.CommentId)
                comments.Remove(comment);
        }
        SaveCommentsAsync(comments);

    }

    public async Task<Comment> GetCommentByIdAsync(int id)
    {
        List<Comment> comments = await LoadCommentsAsync();
        Comment? comment = comments.SingleOrDefault(c => c.CommentId == id);
        if (comment is null)
            throw new InvalidOperationException("No comment found");
        return comment;
    }

    public IQueryable<Comment> GetCommentsForPost(int postId)
    {
        List<Comment> comments = LoadCommentsAsync().Result;

        List<Comment> foundComments =
            comments.FindAll(c => c.Post == postId);
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
        string commentsAsJson = JsonSerializer.Serialize(toSaveComments,
            new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(filePath, commentsAsJson);
    }
}