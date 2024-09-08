using Entities;

namespace RepositoryContracts;

public interface IPostRepository
{
    Post AddPost(Post post);
    Post UpdatePost(Post post);
    Post DeletePost(Post post);
    Post GetPostById(int postId);
    List<Post> GetPosts();
}