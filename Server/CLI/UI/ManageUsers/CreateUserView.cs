using CLI.UI.ManagePosts;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUserView
{
    private IUserRepository userRepository;
    private ICommentRepository commentRepository;
    private IPostRepository postRepository;
    private ILikeRepository likeRepository;
    
    public CreateUserView(IUserRepository userRepository,
        IPostRepository postRepository, ICommentRepository commentRepository,
        ILikeRepository likeRepository)
    {
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
        this.likeRepository = likeRepository;
    }
    public void Open()
    {
        string? username = EnterUsername();
        while (!userRepository.IsUsernameValid(username))
        {
            Console.WriteLine("Invalid username. Please try again.");
            username = EnterUsername();
        }

        string? password = EnterPassword();
        bool created = false;
        while (!created)
        {
            try
            {
                User newUser = userRepository.AddUserAsync(new User
                    { Username = username, Password = password }).Result;
                Console.WriteLine(
                    $"\nUser created successfully: \n   Username: {username}\n   Password: {password}");
                created = true;
                
                ManagePostsView managePostsView = new ManagePostsView(userRepository, postRepository, commentRepository, likeRepository, newUser);
                managePostsView.Open();
                //return Task.CompletedTask;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                password = EnterPassword();
            }
        }
    }

    private string? EnterUsername()
    {
        Console.WriteLine("Enter username:");
        return Console.ReadLine();
    }

    private string? EnterPassword()
    {
        Console.WriteLine("Enter password:");
        return Console.ReadLine();
    }
}