// See https://aka.ms/new-console-template for more information

using InMemoryRepositories;
using RepositoryContracts;

public class Program
{

    public static void Main(string[] args)
    {
        IPostRepository postRepository = new PostInMemoryRepository();
        ICommentRepository commentRepository = new CommentInMemoryRepository();
        IUserRepository userRepository = new UserInMemoryRepository();
        ILikeRepository likeRepository = new LikeInMemoryRepository();
        
        Console.WriteLine(postRepository.ToString());
        Console.WriteLine(commentRepository.ToString());
        Console.WriteLine(userRepository.ToString());
        Console.WriteLine(likeRepository.ToString());
    }
    
}