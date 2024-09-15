using Entities;

namespace RepositoryContracts;

public interface IPostRepository
{
    Task<Post> AddPostAsync(Post post);
    Task UpdatePostAsync(Post post);
    Task DeletePostAsync(int postId);
    Task<Post> GetPostByIdAsync(int postId);
    IQueryable<Post> GetPosts();
}