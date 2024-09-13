using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class PostInMemoryRepository : IPostRepository
{
    private List<Post> posts;

    public PostInMemoryRepository()
    {
        posts = new List<Post>();
        AddPostAsync(new Post{Title = "Solar eclipse today.", Body = "Don't miss today's solar eclipse at 15:37.", UserId = 1});
        AddPostAsync(new Post{Title = "Thread with dad jokes", Body = "Leave your best dad jokes in comments.", UserId = 2});
        
        AddPostAsync(new Post{Title = "Best supervisor at VIA.", Body = "Haha, got your attention.", UserId = 3});
        AddPostAsync(new Post{Title = "Thread with favorite books:", Body = "Lad os begynde", UserId = 3});
        
        AddPostAsync(new Post{Title = "DNP Exam", Body = "No grade other than 12 is acceptable. Can we build it? Ja, selvfoelgelig!", UserId = 4});
        AddPostAsync(new Post{Title = "This is just another post.", Body = "It's Sunday, 20:57 and I'm tired. Oh nej.", UserId = 5});
        AddPostAsync(new Post{Title = "Pain... pain everywhere", Body = "I'm in pain and Panodil doesn't help anymore. What should I do?", UserId = 5});
        AddPostAsync(new Post{Title = "Is the water wet?", Body = "Tell me, please tell me. I need answers", UserId = 5});

    }
    public Task<Post> AddPostAsync(Post post)
    {
        post.PostId = posts.Any() ? posts.Max(p => p.PostId) + 1 : 1;
        posts.Add(post);
        return Task.FromResult(post);
    }

    public Task UpdatePostAsync(Post post)
    {
        Post existingPost = GetPostByIdAsync(post.PostId).Result;
        posts.Remove(existingPost);
        posts.Add(post);
        return Task.CompletedTask;
    }

    public Task DeletePostAsync(int postId)
    {
        Post postToBeDeleted = GetPostByIdAsync(postId).Result;
        posts.Remove(postToBeDeleted);
        return Task.CompletedTask;
    }

    public Task<Post> GetPostByIdAsync(int postId)
    {
        Post? foundPost = posts.SingleOrDefault(p => p.PostId == postId);
        if(foundPost is null)
            throw new InvalidOperationException($"No post with ID {postId} found.");
        return Task.FromResult(foundPost);
    }

    public IQueryable<Post> GetPosts()
    {
        return posts.AsQueryable();
    }
    public string ToString()
    {
        string s = "";
        for(int i=0; i<posts.Count; i++)
            s=s+posts[i].ToString()+'\n';
        return s;
    }
}