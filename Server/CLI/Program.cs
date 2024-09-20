using CLI.UI;
using FileRepositories;
using RepositoryContracts;

Console.WriteLine("Starting CLI app... Vent venligst...");

IPostRepository postRepository = new PostFileRepository(); //PostInMemoryRepository();
ICommentRepository commentRepository = new CommentFileRepository(); //CommentInMemoryRepository();
IUserRepository userRepository = new UserFileRepository(); //UserInMemoryRepository();
ILikeRepository likeRepository = new LikeFileRepository(); //LikeInMemoryRepository();

CliApp cliApp = new CliApp(userRepository, commentRepository, postRepository, likeRepository);
await cliApp.StartAsync();