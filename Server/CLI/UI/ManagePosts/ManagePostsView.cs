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

    public ManagePostsView(IUserRepository userRepository,
        IPostRepository postRepository,
        ICommentRepository commentRepository,
        ILikeRepository likeRepository, User user)
    {
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
        this.likeRepository = likeRepository;
        this.user = user;
    }

    public async Task OpenAsync()
    {
        string? userInput;
        do
        {
            userInput = ChooseAction();
        } while (userInput is null ||
                 (!userInput.Equals("1") && !userInput.Equals("2")));

        if (userInput.Equals("1"))
            await GoToCreatePostAsync();
        else
            await OpenPostAsync();
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
        Console.WriteLine("\n\nPosts:\n");
        for (var i = 0; i < postRepository.GetPosts().Count(); i++)
            Console.WriteLine(
                $"   Post ID: {postRepository.GetPosts().ElementAt(i).PostId} \n{postRepository.GetPosts().ElementAt(i).Title}\n");
    }

    private async Task GoToCreatePostAsync()
    {
        CreatePostView createPostView =
            new CreatePostView(postRepository, user, this);
        await createPostView.OpenAsync();
    }

    private async Task OpenPostAsync()
    {
        Post? foundPost;
        string errorMessage = string.Empty;
        do
        {
            string? postId = ChoosePost(errorMessage);
            foundPost = null;

            try
            {
                foundPost = await postRepository
                    .GetPostByIdAsync(int.Parse(postId));
            }
            catch (InvalidOperationException e)
            {
                errorMessage = e.Message;
            }
            catch (FormatException)
            {
                errorMessage = "Invalid ID.";
            }
        } while (foundPost is null);

        OpenedPostView openedPostView = new OpenedPostView(userRepository,
            commentRepository, likeRepository, user, foundPost,
            this);
        await openedPostView.OpenAsync();
    }
}