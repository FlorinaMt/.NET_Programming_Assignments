using CLI.UI.ManageUsers;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ManagePostsView
{
    private IUserRepository userRepository;
    private ICommentRepository commentRepository;
    private IPostRepository postRepository;
    private ILikeRepository likeRepository;
    private User user;
    
    public ManagePostsView(IUserRepository userRepository, IPostRepository postRepository,
        ICommentRepository commentRepository,
        ILikeRepository likeRepository, User user)
    {
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
        this.likeRepository = likeRepository;
        this.user = user;
    }
    public void Open()
    {
        string? userInput;
        do
        {
            userInput = ChooseAction();
        } while (userInput is null ||
                 (!userInput.Equals("1") && !userInput.Equals("2")));

        if (userInput.Equals("1"))
        {
            CreatePostView createPostView =
                new CreatePostView(postRepository, user, this);
            createPostView.Open();
        }
        else
        {
            string? postId;
            Post? foundPost;
            string errorMessage = string.Empty;
            do
            {
                postId = ChoosePost(errorMessage);
                foundPost = null;
                if (postId is not null)
                {
                    try
                    {
                        foundPost = postRepository
                            .GetPostByIdAsync(int.Parse(postId)).Result;
                    }
                    catch (InvalidOperationException e)
                    {
                        errorMessage = $"ERROR: {e.Message} Try again.";
                    }
                }
            } while (foundPost is null);

            OpenedPostView openedPostView = new OpenedPostView(userRepository, postRepository, commentRepository, likeRepository, user, int.Parse(postId), this);
            openedPostView.Open();
        }
    }

    private string? ChooseAction()
    {
        DisplayPosts();
        Console.WriteLine(
            "\nChoose 1 or 2:\n1. Create a post.\n2. Open a post.");
        return Console.ReadLine();
    }

    private string? ChoosePost(string errorMessage)
    {
        DisplayPosts();
        Console.WriteLine(errorMessage);
        Console.WriteLine("\nEnter post ID:");
        return Console.ReadLine();
    }

    private void DisplayPosts()
    {
        Console.WriteLine("\n\nPosts:");
        for (int i = 0; i < postRepository.GetPosts().Count(); i++)
            Console.WriteLine(
                $"   Post ID: {postRepository.GetPosts().ElementAt(i).PostId} \n{postRepository.GetPosts().ElementAt(i).Title}");
    }
}