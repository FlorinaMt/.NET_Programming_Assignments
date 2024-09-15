using CLI.UI.ManagePosts;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class CreateCommentView
{
    private ICommentRepository commentRepository;
    private User user;
    private int postId;
    private OpenedPostView openedPostView;

    public CreateCommentView(ICommentRepository commentRepository, User user,
        int postId, OpenedPostView openedPostView)
    {
        this.commentRepository = commentRepository;
        this.user = user;
        this.postId = postId;
        this.openedPostView = openedPostView;
    }

    public async Task OpenAsync()
    {
        string? userInput;
        do
        {
            Console.WriteLine("Enter comment text:");
            userInput = Console.ReadLine();
        } while (userInput is null || userInput.Trim().Equals(""));

        await commentRepository.AddCommentAsync(new Comment
            { CommentBody = userInput, UserId = user.UserId, PostId = postId });
        await openedPostView.OpenAsync();
    }
}