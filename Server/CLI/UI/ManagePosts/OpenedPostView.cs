using CLI.UI.ManageComments;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class OpenedPostView
{
    public void Open(IPostRepository postRepository,
        ICommentRepository commentRepository, ILikeRepository likeRepository,
        IUserRepository userRepository, User user, CliApp cliApp, int postId)
    {
        Post post = postRepository.GetPostByIdAsync(postId).Result;

        DisplayCompletePost(post, likeRepository, commentRepository,
            userRepository);

        string userInput = Choose();

        do
        {
            if (userInput.Equals("1"))
            {
                try
                {
                    likeRepository.AddLikeAsync(new Like
                        { UserId = user.UserId, PostId = postId });
                    DisplayCompletePost(post, likeRepository, commentRepository,
                        userRepository);
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(e.Message);
                    userInput = Choose();
                }
            }
            else if (userInput.Equals("2"))
            {
                CreateCommentView createCommentView = new CreateCommentView();
                createCommentView.Open();
            }
        } while (!userInput.Equals("3"));
    }

    private void DisplayCompletePost(Post post, ILikeRepository likeRepository,
        ICommentRepository commentRepository, IUserRepository userRepository)
    {
        Console.WriteLine(
            $"Post ID: {post.PostId}\n {post.Title}\n{post.Body}");
        DisplayLikes(likeRepository, post.PostId);
        DisplayComments(commentRepository, userRepository, post.PostId);
    }

    private void DisplayLikes(ILikeRepository likeRepository, int postId)
    {
        try
        {
            Console.WriteLine(
                $"Likes: {likeRepository.GetLikesForPost(postId).Count()}");
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private void DisplayComments(ICommentRepository commentRepository,
        IUserRepository userRepository, int postId)
    {
        try
        {
            Console.WriteLine("Comments:");
            for (int i = 0;
                 i < commentRepository.GetCommentsForPost(postId).Count();
                 i++)
                Console.WriteLine(
                    $"{userRepository.GetUserByIdAsync(commentRepository.GetCommentsForPost(postId).ElementAt(i).UserId)}:\n {commentRepository.GetCommentsForPost(postId).ElementAt(i).CommentBody}");
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public string Choose()
    {
        Console.WriteLine(
            "Choose 1, 2 or 3:\n1. Like post. \n2. Add comment\n3. Go back to all posts.");
        string? userInput;
        do
        {
            userInput = Console.ReadLine();
        } while (userInput is null || (!userInput.Equals("1") &&
                                       !userInput.Equals("2") &&
                                       !userInput.Equals("3")));

        return userInput;
    }
}