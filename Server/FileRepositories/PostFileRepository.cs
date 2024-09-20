using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository:IPostRepository
{
    private readonly string filePath = "posts.json";

    public PostFileRepository()
    {
        if (!File.Exists(filePath))
            File.WriteAllText(filePath, "[]");
        AddPostAsync(new Post{Title = "Solar eclipse today.", Body = "Don't miss today's solar eclipse at 15:37.", UserId = 1});
        AddPostAsync(new Post{Title = "Thread with dad jokes", Body = "Leave your best dad jokes in comments.", UserId = 2});
        
        AddPostAsync(new Post{Title = "Best supervisor at VIA.", Body = "Haha, got your attention.", UserId = 3});
        AddPostAsync(new Post{Title = "Thread with favorite books:", Body = "Lad os begynde", UserId = 3});
        
        AddPostAsync(new Post{Title = "DNP Exam", Body = "No grade other than 12 is acceptable. Can we build it? Ja, selvfoelgelig!", UserId = 4});
        AddPostAsync(new Post{Title = "This is just another post.", Body = "It's Sunday, 20:57 and I'm tired. Oh nej.", UserId = 5});
        AddPostAsync(new Post{Title = "Pain... pain everywhere", Body = "I'm in pain and Panodil doesn't help anymore. What should I do?", UserId = 5});
        AddPostAsync(new Post{Title = "Is the water wet?", Body = "Tell me, please tell me. I need answers", UserId = 5});

    }

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