using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class LikeFileRepository : ILikeRepository
{
    private readonly string filePath = "likes.json";

    public LikeFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
            AddDummyDataAsync();
        }
    }

    public async Task AddDummyDataAsync()
    {
        int delay = 1000;
        Thread.Sleep(delay);
        await AddLikeAsync(new Like { PostId = 1, UserId = 2 });
        Thread.Sleep(delay);
        await AddLikeAsync(new Like { PostId = 1, UserId = 3 });
        Thread.Sleep(delay);
        await AddLikeAsync(new Like { PostId = 1, UserId = 4 });


        Thread.Sleep(delay);
        await AddLikeAsync(new Like { PostId = 2, UserId = 2 });
        Thread.Sleep(delay);
        await AddLikeAsync(new Like { PostId = 2, UserId = 5 });

        Thread.Sleep(delay);
        await AddLikeAsync(new Like { PostId = 3, UserId = 4 });

        Thread.Sleep(delay);
        await AddLikeAsync(new Like { PostId = 4, UserId = 1 });
        Thread.Sleep(delay);
        await AddLikeAsync(new Like { PostId = 4, UserId = 2 });
        Thread.Sleep(delay);
        await AddLikeAsync(new Like { PostId = 4, UserId = 3 });
        Thread.Sleep(delay);
        await AddLikeAsync(new Like { PostId = 4, UserId = 5 });

        Thread.Sleep(delay);
        await AddLikeAsync(new Like { PostId = 5, UserId = 1 });
        Thread.Sleep(delay);
        await AddLikeAsync(new Like { PostId = 5, UserId = 2 });
        Thread.Sleep(delay);
        await AddLikeAsync(new Like { PostId = 5, UserId = 4 });
    }


    public async Task<Like> AddLikeAsync(Like like)
    {
        List<Like> likes = await LoadLikesAsync();
        if (!likes.Any(l => l.UserId == like.UserId && l.PostId == like.PostId))
        {
            int maxId = likes.Count > 0 ? likes.Max(like => like.LikeId) : 0;
            like.LikeId = maxId + 1;
            likes.Add(like);
            SaveLikesAsync(likes);
        }

        return like;
    }

    public async Task DeleteLikeAsync(int likeId)
    {
        List<Like> likes = await LoadLikesAsync();
        Like likeToDelete = await GetLikeByIdAsync(likeId);
        likes.Remove(likeToDelete);
        SaveLikesAsync(likes);
    }

    public async Task<Like> GetLikeByIdAsync(int id)
    {
        List<Like> likes = await LoadLikesAsync();
        Like? like = likes.SingleOrDefault(l => l.LikeId == id);
        if (like is null)
            throw new InvalidOperationException($"No like with ID {id}.");
        return like;
    }

    public IQueryable<Like> GetLikesForPost(int postId)
    {
        List<Like> likes = LoadLikesAsync().Result;
        List<Like> likesForPost =
            likes.Where(like => like.PostId == postId).ToList();

        return likesForPost.AsQueryable();
    }

    public IQueryable<Like> GetAllLikes()
    {
        List<Like> likes = LoadLikesAsync().Result;
        return likes.AsQueryable();
    }

    private async Task<List<Like>> LoadLikesAsync()
    {
        string likesAsJson = await File.ReadAllTextAsync(filePath);
        List<Like> likes =
            JsonSerializer.Deserialize<List<Like>>(likesAsJson)!;
        return likes;
    }

    private async void SaveLikesAsync(List<Like> toSaveLikes)
    {
        string likesAsJson = JsonSerializer.Serialize(toSaveLikes,
            new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(filePath, likesAsJson);
    }
}