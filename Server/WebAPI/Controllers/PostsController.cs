using ApiContracts;
using ApiContracts.LikeRelated;
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
    public async Task<ActionResult<GetPostResponseDto>> CreatePostAsync(
        [FromBody] CreatePostRequestDto request)
    {
        await userRepository.GetUserByIdAsync(request.UserId);
        Post post = new()
        {
            Title = request.Title, Body = request.Body,
            UserId = request.UserId
        };
        Post created = await postRepository.AddPostAsync(post);
        return Created($"/Posts", created);
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
        foreach (Post post in postRepository.GetPosts())
            sendDto.Add(await GetPostByIdAsync(post.PostId));

        //filter them
        if (!string.IsNullOrEmpty(author))
            sendDto = sendDto.Where(p =>
                p.AuthorUsername.ToLower().Equals(author.ToLower())).ToList();

        if (sendDto.Count == 0)
            throw new ArgumentException("No posts found.");

        return Created($"/Posts", sendDto);
    }

    private async Task<GetPostResponseDto> GetPostByIdAsync(int postId)
    {
        Post post = await postRepository.GetPostByIdAsync(postId);

        //extract post's author username
        String authorUsername =
            (await userRepository.GetUserByIdAsync(post.UserId)).Username;

        //extract the likes
        List<Like> likesForPost =
            likeRepository.GetLikesForPost(post.PostId).ToList();

        //extract the number of likes
        int likesNo = likesForPost.Count;

        //extract the elements as DTOs
        List<GetLikeDto> likes = new();

        foreach (Like l in likesForPost)
            likes.Add(new GetLikeDto
            {
                LikeId = l.LikeId, PostId = post.PostId,
                Username = (await userRepository.GetUserByIdAsync(l.UserId)).Username
            });


        //extract the comments
        List<GetCommentResponseDto> comments = new();

        foreach (Comment comment in commentRepository.GetCommentsForPost(
                     post.PostId))
            comments.Add(new GetCommentResponseDto
            {
                CommentId = comment.CommentId,
                Body = comment.CommentBody, PostId = postId,
                AuthorUsername =
                    (await userRepository.GetUserByIdAsync(comment.UserId))
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
        [FromBody] DeleteRequestDto request, [FromQuery] string? title,
        [FromQuery] string? body, [FromRoute] int id)
    {
        //get the post
        Post post = await postRepository.GetPostByIdAsync(id);

        //if this is the author, they can update it
        if (post.UserId == request.UserId)
        {
            Post newPost = new Post
            {
                Title = title, Body = body, UserId = request.UserId,
                PostId = request.ItemToDeleteId
            };

            await postRepository.UpdatePostAsync(newPost);

            GetPostResponseDto updatedPost =
                await GetPostByIdAsync(request.ItemToDeleteId);


            return Created($"/Posts/{updatedPost.PostId}", updatedPost);
        }

        throw new ArgumentException("Only the author can update this post.");
    }

    [HttpDelete("{id}")]
    public async Task<IResult> DeletePostAsync(
        [FromBody] DeleteRequestDto request, [FromRoute] int id)
    {
        Post post = await postRepository.GetPostByIdAsync(request.ItemToDeleteId);
        if (post.UserId == request.UserId)
        {
            await postRepository.DeletePostAsync(request.ItemToDeleteId);
            return Results.NoContent();
        }

        throw new ArgumentException("Only the author can delete this post.");
    }

    //--------------------------------ADD LIKE---------------------------------
    [HttpPost("{id}/Likes")]
    public async Task<ActionResult<GetLikeDto>> AddLikeAsync(
        AddLikeRequestDto request, [FromRoute] int id)
    {
        await userRepository.GetUserByIdAsync(request.UserId);
        await postRepository.GetPostByIdAsync(id);

        foreach (Like like in likeRepository.GetLikesForPost(id))
            if (like.UserId == request.UserId)
                throw new ArgumentException("You already liked this post.");

        Like newLike = new Like
            { UserId = request.UserId, PostId = id };
        Like addedLike = await likeRepository.AddLikeAsync(newLike);

        GetLikeDto created = new GetLikeDto
        {
            LikeId = addedLike.LikeId, PostId = id,
            Username = (await userRepository.GetUserByIdAsync(addedLike.UserId))
                .Username
        };
        return Created($"/Posts/{id}/Likes", created);
    }

    //------------------------------ADD COMMENT--------------------------------

    [HttpPost("{id}/Comments")]
    public async Task<ActionResult<GetCommentResponseDto>> AddCommentAsync(
        CreateCommentRequestDto request, [FromRoute] int id)
    {
        await userRepository.GetUserByIdAsync(request.UserId);
        await postRepository.GetPostByIdAsync(id);

        Comment newComment = new Comment
        {
            CommentBody = request.Body, PostId = id, UserId = request.UserId
        };

        Comment addedComment =
            await commentRepository.AddCommentAsync(newComment);

        GetCommentResponseDto created = new GetCommentResponseDto
        {
            CommentId = addedComment.CommentId, Body = addedComment.CommentBody,
            PostId = id,
            AuthorUsername =
                (await userRepository.GetUserByIdAsync(addedComment.UserId))
                .Username
        };

        return Created($"/Posts/{id}/Comments", created);
    }
}