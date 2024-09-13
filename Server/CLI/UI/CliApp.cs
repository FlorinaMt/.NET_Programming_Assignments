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

    public Task StartAsync()
    {
        return LoadManageUsersView();
    }

    public Task LoadManageUsersView()
    {
        ManageUsersView manageUsersView = new ManageUsersView();
        manageUsersView.Open(userRepository, this);
        return Task.CompletedTask;
    }
    public Task LoadManagePostsView(User user)
    {
        ManagePostsView managePostsView = new ManagePostsView();
        managePostsView.Open(postRepository, user, this);
        return Task.CompletedTask;
    }
}