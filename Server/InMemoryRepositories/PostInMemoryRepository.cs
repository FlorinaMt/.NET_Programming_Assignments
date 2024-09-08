using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class PostInMemoryRepository : IPostRepository
{
    public List<Post> posts = new List<Post>();
    public Task<Post> AddPostAsync(Post post)
    {
        post.PostId = posts.Any() ? posts.Max(p => p.PostId) + 1 : 1;
        posts.Add(post);
        return Task.FromResult(post);
    }

    public Task UpdatePostAsync(Post post)
    {
        Post? existingPost =
            posts.SingleOrDefault(p => p.PostId == post.PostId);
        if(existingPost is null)
            throw new InvalidOperationException($"No post with ID {post.PostId} found.");
        posts.Remove(existingPost);
        posts.Add(post);
        return Task.CompletedTask;
    }

    public Task DeletePostAsync(int postId)
    {
        Post? postToBeDeleted = posts.SingleOrDefault(p => p.PostId == postId);
        if(postToBeDeleted is null)
            throw new InvalidOperationException($"No post with ID {postId} found.");
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
}