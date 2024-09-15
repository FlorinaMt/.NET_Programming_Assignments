using CLI.UI.ManageComments;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class OpenedPostView
{
    private IUserRepository userRepository;
    private ICommentRepository commentRepository;
    private IPostRepository postRepository;
    private ILikeRepository likeRepository;

    private ManagePostsView managePostsView;
    private User user;
    private int postId;

    public OpenedPostView(IUserRepository userRepository,
        IPostRepository postRepository, ICommentRepository commentRepository,
        ILikeRepository likeRepository, User user, int postId,
        ManagePostsView managePostsView)
    {
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
        this.likeRepository = likeRepository;

        this.user = user;
        this.postId = postId;
        this.managePostsView = managePostsView;
    }

    public void Open()
    {
        Post post = postRepository.GetPostByIdAsync(postId).Result;

        DisplayCompletePost(post);

        string userInput = Choose();

        do
        {
            if (userInput.Equals("1"))
            {
                try
                {
                    likeRepository.AddLikeAsync(new Like
                        { UserId = user.UserId, PostId = postId });
                    DisplayCompletePost(post);
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(e.Message);
                    userInput = Choose();
                }
            }
            else if (userInput.Equals("2"))
            {
                CreateCommentView createCommentView = new CreateCommentView(commentRepository, user, postId, this);
                createCommentView.Open();
            }
        } while (!userInput.Equals("3"));

        managePostsView.Open();
    }

    private void DisplayCompletePost(Post post)
    {
        Console.WriteLine(
            $"Post ID: {post.PostId}\n {post.Title}\n{post.Body}");
        DisplayLikes();
        DisplayComments();
    }

    private void DisplayLikes()
    {
        try
        {
            Console.WriteLine(
                $"\nLikes: {likeRepository.GetLikesForPost(postId).Count()}");
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private void DisplayComments()
    {
        try
        {
            Console.WriteLine("\nComments:");
            for (int i = 0;
                 i < commentRepository.GetCommentsForPost(postId).Count();
                 i++)
                Console.WriteLine(
                    $"{userRepository.GetUserByIdAsync(commentRepository.GetCommentsForPost(postId).ElementAt(i).UserId).Result.Username}:\n   {commentRepository.GetCommentsForPost(postId).ElementAt(i).CommentBody}");
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public string Choose()
    {
        string? userInput;
        do
        {
            Console.WriteLine(
                "\nChoose 1, 2 or 3:\n1. Like post. \n2. Add comment\n3. Go back to all posts.");
            userInput = Console.ReadLine();
        } while (userInput is null || (!userInput.Equals("1") &&
                                       !userInput.Equals("2") &&
                                       !userInput.Equals("3")));

        return userInput;
    }
}