using CLI.UI;
using InMemoryRepositories;
using RepositoryContracts;

Console.WriteLine("Starting CLI app... Vent venligst...");
IPostRepository postRepository = new PostInMemoryRepository();
ICommentRepository commentRepository = new CommentInMemoryRepository();
IUserRepository userRepository = new UserInMemoryRepository();
ILikeRepository likeRepository = new LikeInMemoryRepository();

CliApp cliApp=new CliApp(userRepository, commentRepository, postRepository, likeRepository);
await cliApp.StartAsync();