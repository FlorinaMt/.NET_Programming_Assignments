using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView
{
    
    public void Open(IPostRepository postRepository, User user, CliApp cliApp)
    {
        string? title;
        do
        {
            Console.WriteLine("Enter Post Title:");
            title = Console.ReadLine();
        } while (title is null);
        string? body;
        do
        {
            Console.WriteLine("Enter Post Body:");
            body = Console.ReadLine();
        } while (body is null);

        postRepository.AddPostAsync(new Post { Title = title, Body = body, UserId = user.UserId});
        Console.WriteLine("Post Created Successfully.");
        cliApp.LoadManagePostsView(user);
    }
}