using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;
    private readonly ILikeRepository likeRepository;
    private readonly ICommentRepository commentRepository;


    public PostsController(IPostRepository postRepository,
        IUserRepository userRepository, ILikeRepository likeRepository,
        ICommentRepository commentRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
        this.likeRepository = likeRepository;
        this.commentRepository = commentRepository;
    }

    [HttpPost]
    public async Task<ActionResult<Post>> CreatePost(
        [FromBody] CreatePostRequestDto request)
    {
        Post post = new()
        {
            Title = request.Title, Body = request.Body, UserId = request.UserId
        };
        Post created = await postRepository.AddPostAsync(post);
        return Created($"/Posts/{created.PostId}", created);
    }

    [HttpGet]
    public async Task<ActionResult<List<GetPostResponseDto>>> GetPost(
        [FromQuery] int id)
    {
        List<GetPostResponseDto> sendDto = new List<GetPostResponseDto>();
        
        //if we have an id, we display that post
        if (id != 0)
        {
            sendDto.Add(await GetPostIdAsync(id));
            return Created($"/Posts/{id}", sendDto);
        }
        
        //otherwise, we display all posts
        foreach (Post post in postRepository.GetPosts())
            sendDto.Add(await GetPostIdAsync(post.PostId));
        return Created($"/Posts", sendDto);
    }

    private async Task<GetPostResponseDto> GetPostIdAsync(int postId)
    {
        Post post = await postRepository.GetPostByIdAsync(postId);

        //extract post's author username
        String authorUsername =
            (await userRepository.GetUserByIdAsync(post.UserId)).Username;

        //extract the number of likes
        int likesNo = likeRepository.GetLikesForPost(post.PostId).Count();

        //extract the comments
        List<GetCommentResponseDto> comments = new();

        foreach (Comment comment in commentRepository.GetCommentsForPost(
                     post.PostId))
            comments.Add(new GetCommentResponseDto
            {
                Body = comment.CommentBody, PostId = comment.PostId,
                AuthorUsername =
                    (await userRepository.GetUserByIdAsync(comment.UserId))
                    .Username
            });

        GetPostResponseDto sendDto = new()
        {
            Title = post.Title, Body = post.Body, PostId = post.PostId,
            AuthorUsername = authorUsername,
            LikesNo = likesNo,
            Comments = comments
        };
        return sendDto;
    }
}