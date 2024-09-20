using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository:IPostRepository
{
    private readonly string filePath = "posts.json";
    
    public async Task<Post> AddPostAsync(Post post)
    {
        List<Post> posts = await LoadPostsAsync();
        int maxId = posts.Count > 0 ? posts.Max(p => p.PostId) : 1;
        post.PostId = maxId + 1;
        posts.Add(post);
        SavePostsAsync(posts);
        return post;
    }

    public async Task UpdatePostAsync(Post post)
    {
        List<Post> posts = await LoadPostsAsync();
        Post postToUpdate = await GetPostByIdAsync(post.PostId);
        posts.Remove(postToUpdate);
        posts.Add(post);
        SavePostsAsync(posts);
    }

    public async Task DeletePostAsync(int postId)
    {
        List<Post> posts = await LoadPostsAsync();
        Post postToDelete = await GetPostByIdAsync(postId);
        posts.Remove(postToDelete);
    }

    public async Task<Post> GetPostByIdAsync(int postId)
    {
        List<Post> posts = await LoadPostsAsync();
        Post? post = posts.SingleOrDefault(p => p.PostId == postId);
        if (post is null)
            throw new InvalidOperationException($"No post with ID {postId}.");
        return post;
    }

    public IQueryable<Post> GetPosts()
    {
        List<Post> posts = LoadPostsAsync().Result;
        return posts.AsQueryable();
    }
    private async Task<List<Post>> LoadPostsAsync()
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts =
            JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        return posts;
    }

    private async void SavePostsAsync(List<Post> toSavePosts)
    {
        string postsAsJson = JsonSerializer.Serialize(toSavePosts);
        await File.WriteAllTextAsync(filePath, postsAsJson);
    }
}