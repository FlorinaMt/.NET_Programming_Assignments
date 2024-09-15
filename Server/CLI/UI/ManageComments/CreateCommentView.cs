using CLI.UI.ManagePosts;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class CreateCommentView
{
    private ICommentRepository commentRepository;
    private OpenedPostView openedPostView;

    private User user;
    private int postId;

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
        bool created = false;
        while (created is false)
        {
            try
            {
                Console.WriteLine("Enter comment text:");
                string? userInput = Console.ReadLine();

                await commentRepository.AddCommentAsync(new Comment
                {
                    CommentBody = userInput, UserId = user.UserId,
                    PostId = postId
                });
                created = true;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        await openedPostView.OpenAsync();
    }
}