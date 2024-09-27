using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class ReactionFileRepository : IReactionRepository
{
    private readonly string filePath = "reactions.json";

    public ReactionFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
            int delay = 100;
            Thread.Sleep(delay);
            AddReactionAsync(new Reaction { CommentId = 1, UserId =  1, IsPositive = true});
            Thread.Sleep(delay);
            AddReactionAsync(new Reaction { CommentId = 1, UserId =  2, IsPositive = false});
            Thread.Sleep(delay);
            AddReactionAsync(new Reaction { CommentId = 1, UserId =  3, IsPositive = true});
            Thread.Sleep(delay);
            AddReactionAsync(new Reaction { CommentId = 2, UserId =  4, IsPositive = true});
            Thread.Sleep(delay);
            AddReactionAsync(new Reaction { CommentId = 3, UserId =  5, IsPositive = true});
            Thread.Sleep(delay);
            AddReactionAsync(new Reaction { CommentId = 4, UserId =  1, IsPositive = false});
            Thread.Sleep(delay);
            AddReactionAsync(new Reaction { CommentId = 5, UserId =  2, IsPositive = false});
            Thread.Sleep(delay);
            AddReactionAsync(new Reaction { CommentId = 6, UserId =  3, IsPositive = true});
            Thread.Sleep(delay);
            AddReactionAsync(new Reaction { CommentId = 6, UserId =  4, IsPositive = false});
            Thread.Sleep(delay);
            AddReactionAsync(new Reaction { CommentId = 7, UserId =  5, IsPositive = true});
            Thread.Sleep(delay);
            AddReactionAsync(new Reaction { CommentId = 8, UserId =  1, IsPositive = false});
            Thread.Sleep(delay);
            AddReactionAsync(new Reaction { CommentId = 9, UserId =  2, IsPositive = false});
            Thread.Sleep(delay);
            AddReactionAsync(new Reaction { CommentId = 9, UserId =  3, IsPositive = true});
            Thread.Sleep(delay);
            AddReactionAsync(new Reaction { CommentId = 9, UserId =  4, IsPositive = false});
            Thread.Sleep(delay);
            AddReactionAsync(new Reaction { CommentId = 10, UserId =  5, IsPositive = true});
            Thread.Sleep(delay);
            AddReactionAsync(new Reaction { CommentId = 11, UserId =  1, IsPositive = false});
            Thread.Sleep(delay);
            AddReactionAsync(new Reaction { CommentId = 12, UserId =  2, IsPositive = true});
            Thread.Sleep(delay);
            AddReactionAsync(new Reaction { CommentId = 13, UserId =  3, IsPositive = true});
            Thread.Sleep(delay);
            AddReactionAsync(new Reaction { CommentId = 14, UserId =  4, IsPositive = true});
            Thread.Sleep(delay);
            AddReactionAsync(new Reaction { CommentId = 15, UserId =  5, IsPositive = true});
            Thread.Sleep(delay);
            AddReactionAsync(new Reaction { CommentId = 15, UserId =  1, IsPositive = false});
            Thread.Sleep(delay);
            AddReactionAsync(new Reaction { CommentId = 16, UserId =  2, IsPositive = true});
            Thread.Sleep(delay);
            AddReactionAsync(new Reaction { CommentId = 17, UserId =  3, IsPositive = true});
            Thread.Sleep(delay);
            AddReactionAsync(new Reaction { CommentId = 17, UserId =  4, IsPositive = false});
            Thread.Sleep(delay);
            AddReactionAsync(new Reaction { CommentId = 18, UserId =  5, IsPositive = true});
            Thread.Sleep(delay);
            AddReactionAsync(new Reaction { CommentId = 19, UserId =  1, IsPositive = true});
            Thread.Sleep(delay);
            AddReactionAsync(new Reaction { CommentId = 20, UserId =  2, IsPositive = false});
            Thread.Sleep(delay);
            AddReactionAsync(new Reaction { CommentId = 21, UserId =  3, IsPositive = true});
        }
        
    }
    public async Task<Reaction> AddReactionAsync(Reaction reaction)
    {
        List<Reaction> reactions = await LoadReactionsAsync();
        if (!reactions.Any(r =>  r.UserId == reaction.UserId && r.CommentId == reaction.CommentId))
        {
            int maxId = reactions.Count > 0 ? reactions.Max(r => r.ReactionId) : 0;
            reaction.ReactionId = maxId + 1;
            reactions.Add(reaction);
            SaveReactionsAsync(reactions);
        }
        return reaction;
    }


    public async Task DeleteReactionAsync(Reaction reaction)
    {
        List<Reaction> reactions = await LoadReactionsAsync();
        Reaction reactionToDelete = await GetReactionByIdAsync(reaction.ReactionId);
        reactions.Remove(reactionToDelete);
        SaveReactionsAsync(reactions);
    }

    public async Task<Reaction> GetReactionByIdAsync(int id)
    {
        List<Reaction> reactions = await LoadReactionsAsync();
        Reaction? reaction = reactions.SingleOrDefault(r => r.ReactionId==id);
        if (reaction is null)
            throw new InvalidOperationException($"No reaction with ID {id}.");
        return reaction;
    }

    public IQueryable<Reaction> GetReactionsForComment(int commentId)
    {
        List<Reaction> reactions = LoadReactionsAsync().Result;
        List<Reaction> reactionsForComment =
            reactions.Where(reaction => reaction.CommentId==commentId).ToList();
        if (reactionsForComment.Count == 0)
            throw new InvalidOperationException("No reactions for this comment.");

        return reactionsForComment.AsQueryable();
    }

    public IQueryable<Reaction> GetAllReactions()
    {
        List<Reaction> reactions = LoadReactionsAsync().Result;
        return reactions.AsQueryable();
    }

    private async Task<List<Reaction>> LoadReactionsAsync()
    {
        string reactionsAsJson = await File.ReadAllTextAsync(filePath);
        List<Reaction> reactions =
            JsonSerializer.Deserialize<List<Reaction>>(reactionsAsJson)!;
        return reactions;
    }

    private async void SaveReactionsAsync(List<Reaction> toSaveReactions)
    {
        string reactionsAsJson = JsonSerializer.Serialize(toSaveReactions,
            new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(filePath, reactionsAsJson);
    }

}