using ApiContracts;
using ApiContracts.LikeRelated;
using Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public async Task<ActionResult<GetPostResponseDto>> CreatePostAsync(
        [FromBody] CreatePostRequestDto request)
    {
        User user = await userRepository.GetUserByIdAsync(request.UserId);
        Post post = Post.getPost();
        post.Title = request.Title;
        post.Body = request.Body;
        post.User = user;

        Post created = await postRepository.AddPostAsync(post);
        GetPostResponseDto sendDto = new GetPostResponseDto
        {
            AuthorUsername = created.User.Username, PostId = created.PostId,
            Body = created.Body, Title = created.Title
        };

        return Created($"/Posts", sendDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetPostResponseDto>> GetPostAsync(
        [FromRoute] int id)
    {
        GetPostResponseDto sendDto = await GetPostByIdAsync(id);
        return Created($"/Posts/{id}", sendDto);
    }

    [HttpGet]
    public async Task<ActionResult<List<GetPostResponseDto>>> GetPostsAsync(
        [FromQuery] string? author)
    {
        List<GetPostResponseDto> sendDto = new List<GetPostResponseDto>();

        //extracts all posts
        foreach (Post post in await postRepository.GetPosts().ToListAsync())
            sendDto.Add(await GetPostByIdAsync(post.PostId));

        //filter them
        if (!string.IsNullOrEmpty(author))
            sendDto = sendDto.Where(p =>
                p.AuthorUsername.ToLower().Contains(author.ToLower())).ToList();

        if (sendDto.Count == 0)
            throw new ArgumentException("No posts found.");

        return Ok(sendDto);
    }

    private async Task<GetPostResponseDto> GetPostByIdAsync(int postId)
    {
        Post post = await postRepository.GetPostByIdAsync(postId);

        //extract post's author username
        //String authorUsername = post.User.Username;
        String authorUsername =
            (await userRepository.GetUserByIdAsync(post.User.UserId)).Username;

        //extract the likes
        //List<Like>? likesForPost = post.Likes;
        List<Like>? likesForPost =
            await likeRepository.GetLikesForPost(post.PostId).ToListAsync();

        //extract the number of likes
        int likesNo = 0;

        //extract the elements as DTOs
        List<GetLikeDto> likes = new();

        if (likesForPost != null)
        {
            likesNo = likesForPost.Count;

            foreach (Like l in likesForPost)
                likes.Add(new GetLikeDto
                {
                    LikeId = l.LikeId, PostId = post.PostId,
                    Username = (await userRepository.GetUserByIdAsync(l.User.UserId)).Username
                });
        }

        //extract the comments
        List<GetCommentResponseDto> comments = new();

        foreach (Comment comment in await commentRepository.GetCommentsForPost(
                     post.PostId).ToListAsync())
            comments.Add(new GetCommentResponseDto
            {
                CommentId = comment.CommentId,
                Body = comment.CommentBody, PostId = postId,
                AuthorId = comment.User.UserId,
                AuthorUsername =
                    (await userRepository.GetUserByIdAsync(comment.User.UserId))
                    .Username
            });

        GetPostResponseDto sendDto = new()
        {
            Title = post.Title, Body = post.Body, PostId = post.PostId,
            AuthorUsername = authorUsername,
            LikesNo = likesNo,
            Likes = likes,
            Comments = comments
        };

        return sendDto;
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<GetPostResponseDto>> ReplacePostAsync(
        [FromQuery] string? title,
        [FromQuery] string? body, [FromRoute] int id)
    {
        //get the post
        Post post = await postRepository.GetPostByIdAsync(id);

        Post newPost = Post.getPost();
        if (!string.IsNullOrWhiteSpace(title))
            newPost.Title = title;
        else newPost.Title = post.Title;

        if (!string.IsNullOrWhiteSpace(body))
            newPost.Body = body;
        else newPost.Body = post.Body;

        newPost.User =
            await userRepository.GetUserByIdAsync(post.User.UserId);
        newPost.PostId = id;

        await postRepository.UpdatePostAsync(newPost);

        GetPostResponseDto updatedPost =
            await GetPostByIdAsync(id);

        return Created($"/Posts/{updatedPost.PostId}", updatedPost);
    }


    [HttpDelete("{id}")]
    public async Task<IResult> DeletePostAsync([FromRoute] int id)
    {
        await postRepository.DeletePostAsync(id);
        return Results.NoContent();
    }

//--------------------------------ADD LIKE---------------------------------
    [HttpPost("{id}/Likes")]
    public async Task<ActionResult<GetLikeDto>> AddLikeAsync(
        AddLikeRequestDto request, [FromRoute] int id)
    {
        User user = await userRepository.GetUserByIdAsync(request.UserId);
        Post post = await postRepository.GetPostByIdAsync(id);

        Like newLike = Like.getLike();
        newLike.User = user;
        newLike.Post = post;

        foreach (Like like in await likeRepository.GetLikesForPost(id).ToListAsync())
            if (like.User.UserId == request.UserId)
                throw new ArgumentException("You already liked this post.");

        Like addedLike = await likeRepository.AddLikeAsync(newLike);

        GetLikeDto created = new GetLikeDto
        {
            LikeId = addedLike.LikeId, PostId = addedLike.LikeId,
            Username = addedLike.User.Username
        };
        return Created($"/Posts/{id}/Likes", created);
    }

//------------------------------ADD COMMENT--------------------------------

    [HttpPost("{id}/Comments")]
    public async Task<ActionResult<GetCommentResponseDto>> AddCommentAsync(
        CreateCommentRequestDto request, [FromRoute] int id)
    {
        User user = await userRepository.GetUserByIdAsync(request.UserId);
        Post post = await postRepository.GetPostByIdAsync(id);

        Comment newComment = Comment.getComment();
        newComment.Post = post;
        newComment.User = user;
        newComment.CommentBody = request.Body;

        Comment addedComment =
            await commentRepository.AddCommentAsync(newComment);

        GetCommentResponseDto created = new GetCommentResponseDto
        {
            CommentId = addedComment.CommentId, Body = addedComment.CommentBody,
            PostId = addedComment.Post.PostId,
            AuthorUsername = addedComment.User.Username,
            AuthorId = addedComment.User.UserId
        };

        return Created($"/Posts/{id}/Comments", created);
    }
}