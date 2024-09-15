using CLI.UI.ManageUsers;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView
{
    private IPostRepository postRepository;
    private ManagePostsView managePostsView;
    private User user;

    public CreatePostView(IPostRepository postRepository, User user,
        ManagePostsView managePostsView)
    {
        this.postRepository = postRepository;
        this.user = user;
        this.managePostsView = managePostsView;
    }

    public async Task OpenAsync()
    {
        Post post = new Post{ Title = "1", Body = "1", UserId = user.UserId };
        bool updated = false;
        while (updated is false)
        {
            try
            {
                Console.WriteLine("Enter post title:");
                post.Title = Console.ReadLine();
                updated = true;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        updated = false;
        while (updated is false)
        {
            try
            {
                Console.WriteLine("Enter post body:");
                post.Body = Console.ReadLine();
                updated = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        await postRepository.AddPostAsync(post);
        Console.WriteLine("Post created successfully.");

        await managePostsView.OpenAsync();

    }
}