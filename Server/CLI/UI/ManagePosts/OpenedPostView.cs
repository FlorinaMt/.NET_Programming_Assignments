using CLI.UI.ManageComments;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class OpenedPostView
{
    private IUserRepository userRepository;
    private ICommentRepository commentRepository;
    private ILikeRepository likeRepository;

    private ManagePostsView managePostsView;
    private User user;
    private Post post;

    public OpenedPostView(IUserRepository userRepository,
        ICommentRepository commentRepository,
        ILikeRepository likeRepository, User user, Post post,
        ManagePostsView managePostsView)
    {
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
        this.likeRepository = likeRepository;

        this.user = user;
        this.post = post;
        this.managePostsView = managePostsView;
    }

    public async Task OpenAsync()
    {
        await DisplayCompletePostAsync();

        string userInput = Choose();

        do
        {
            if (userInput.Equals("1"))
            {
                try
                {
                    await likeRepository.AddLikeAsync(new Like
                        { UserId = user.UserId, PostId = post.PostId });
                    await DisplayCompletePostAsync();
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(e.Message);
                    userInput = Choose();
                }
            }
            else if (userInput.Equals("2"))
                await CommentAsync();
        } while (!userInput.Equals("3"));

        await managePostsView.OpenAsync();
    }

    private async Task DisplayCompletePostAsync()
    {
        Console.WriteLine(
            $"Post ID: {post.PostId}\n {post.Title}\n{post.Body}");
        await DisplayLikesAsync();
        await DisplayCommentsAsync();
    }

    private async Task DisplayLikesAsync()
    {
        try
        {
            Console.WriteLine(
                $"\nLikes: {likeRepository.GetLikesForPost(post.PostId).Count()}, from: ");
            for (var i = 0;
                 i < likeRepository.GetLikesForPost(post.PostId).Count();
                 i++)
                Console.WriteLine((await userRepository.GetUserByIdAsync(
                    likeRepository.GetLikesForPost(post.PostId).ElementAt(i)
                        .UserId)).Username);
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private async Task DisplayCommentsAsync()
    {
        try
        {
            Console.WriteLine("\nComments:");
            for (int i = 0;
                 i < commentRepository.GetCommentsForPost(post.PostId).Count();
                 i++)
                Console.WriteLine(
                    $"{(await userRepository.GetUserByIdAsync(commentRepository.GetCommentsForPost(post.PostId).ElementAt(i).UserId)).Username}:\n   {commentRepository.GetCommentsForPost(post.PostId).ElementAt(i).CommentBody}");
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private string Choose()
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


    private async Task CommentAsync()
    {
        CreateCommentView createCommentView =
            new CreateCommentView(commentRepository, user, post.PostId, this);
        await createCommentView.OpenAsync();
    }
}