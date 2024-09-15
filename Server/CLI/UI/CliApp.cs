using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using Entities;
using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    private IUserRepository userRepository;
    private ICommentRepository commentRepository;
    private IPostRepository postRepository;
    private ILikeRepository likeRepository;
    
    public CliApp(IUserRepository userRepository,
        ICommentRepository commentRepository, IPostRepository postRepository,
        ILikeRepository likeRepository)
    {
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
        this.likeRepository = likeRepository;
    }

    public async Task StartAsync()
    {
        ManageUsersView manageUsersView = new ManageUsersView(userRepository, postRepository, commentRepository, likeRepository);
                await manageUsersView.OpenAsync();
    }
}