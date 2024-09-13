using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class CreateCommentView
{
    public void Open(ICommentRepository commentRepository, User user, int postId)
    {
        string? userInput;
        do
        {
            Console.WriteLine("Enter comment text:\n");
            userInput = Console.ReadLine();
        } while (userInput is null);
        commentRepository.AddCommentAsync(new Comment{CommentBody = userInput, UserId = user.UserId, PostId = postId});
    }
}