using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ManagePostsView
{
    public void Open(IPostRepository postRepository, ICommentRepository commentRepository, ILikeRepository likeRepository, IUserRepository userRepository, User user, CliApp cliApp)
    {
        string? userInput;
        do
        { 
            userInput = ChooseAction(postRepository);
        } while (userInput is null || (!userInput.Equals("1") && !userInput.Equals("2")));

        if (userInput.Equals("1"))
        {
            CreatePostView createPostView =
                new CreatePostView();
            createPostView.Open(postRepository, user, cliApp);
        }
        else
        {
            string? postId;
            do
            {
                postId = ChoosePost(postRepository);
            }while(postId is null || postRepository.GetPostByIdAsync(int.Parse(postId)).Result == null);

            OpenedPostView openedPostView = new OpenedPostView();
            openedPostView.Open(postRepository, commentRepository, likeRepository, userRepository, user, cliApp, int.Parse(postId));
        }
    }

    private string? ChooseAction(IPostRepository postRepository)
    {
        DisplayPosts(postRepository);
        Console.WriteLine("Choose 1 or 2:\n1. Create a post.\n2. Open a post.");
        return Console.ReadLine();
    }
    private string? ChoosePost(IPostRepository postRepository)
    {
        DisplayPosts(postRepository);
        Console.WriteLine("Choose 1 or 2:\n1. Create a post.\n2. Open a post.");
        return Console.ReadLine();
    }

    private void DisplayPosts(IPostRepository postRepository)
    {
        for(int i=0; i<postRepository.GetPosts().Count(); i++)
            Console.WriteLine($"Post ID: {postRepository.GetPosts().ElementAt(i).PostId} \n{postRepository.GetPosts().ElementAt(i).Title}");
    }
}