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
        
        Console.WriteLine(postRepository.GetPosts());
        Console.WriteLine(commentRepository.GetAllComments());
        Console.WriteLine(userRepository.GetUsers());
    }
}