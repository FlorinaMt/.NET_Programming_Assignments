using CLI.UI.ManageUsers;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView
{
    private IPostRepository postRepository;
    private User user;
    private ManagePostsView managePostsView;
    
    public CreatePostView(IPostRepository postRepository, User user,
        ManagePostsView managePostsView)
    {
        this.postRepository = postRepository;
        this.user = user;
        this.managePostsView = managePostsView;
    }
    
    public void Open()
    {
        string? title;
        do
        {
            Console.WriteLine("Enter post title:");
            title = Console.ReadLine();
        } while (title is null);
        string? body;
        do
        {
            Console.WriteLine("Enter post body:");
            body = Console.ReadLine();
        } while (body is null);

        postRepository.AddPostAsync(new Post { Title = title, Body = body, UserId = user.UserId});
        Console.WriteLine("Post created successfully.");

        managePostsView.Open();
    }
}