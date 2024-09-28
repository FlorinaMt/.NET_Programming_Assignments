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
        await userRepository.GetUserByIdAsync(request.UserId);
            Post post = new()
            {
                Title = request.Title, Body = request.Body,
                UserId = request.UserId
            };
            Post created = await postRepository.AddPostAsync(post);
            return Created($"/Posts/{created.PostId}", created);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetPostResponseDto>> GetPost(
        [FromRoute] int id)
    {
        GetPostResponseDto sendDto = await GetPostIdAsync(id);
        return Created($"/Posts/{id}", sendDto);
    }
    
    [HttpGet]
    public async Task<ActionResult<List<GetPostResponseDto>>> GetPosts(
        [FromQuery] string? author)
    {
        List<GetPostResponseDto> sendDto = new List<GetPostResponseDto>();
        
        //extracts all posts
        foreach (Post post in postRepository.GetPosts())
            sendDto.Add(await GetPostIdAsync(post.PostId));

        //filter them
        if (!string.IsNullOrEmpty(author))
            sendDto = sendDto.Where(p => p.AuthorUsername.ToLower().Equals(author.ToLower())).ToList();

        if (sendDto.Count == 0)
            throw new ArgumentException("No posts found.");
        
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

    [HttpPut]
    public async Task<ActionResult<GetPostResponseDto>> ReplacePost([FromBody]
        DeleteRequestDto request, [FromQuery] string? title, [FromQuery] string? body)
    {
        //get the post
        Post post = await postRepository.GetPostByIdAsync(request.ItemId);
        
        //if this is the author, they can update it
        if (post.UserId == request.UserId)
        {
            Post newPost = new Post
            {
                Title = title, Body = body, UserId = request.UserId,
                PostId = request.ItemId
            };

            await postRepository.UpdatePostAsync(newPost);
            
            GetPostResponseDto updatedPost =
                await GetPostIdAsync(request.ItemId);
            
            
            return Created($"/Posts/{updatedPost.PostId}", updatedPost);
        }
        
        throw new ArgumentException("Only the author can update this post.");
    }
    
}