using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace WebAPI.Controllers;

/*
 * _Users - POST(username, password) -> (username, id);
         GET (?nameContains) -> (username[]);
         PUT (userId, username, password) -> (username, id) /*reuse from POST*\;
         DELETE (userId, password) -> ();
 */

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepository;

    public UsersController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    [HttpPost]
    public async Task<ActionResult<AddUserResponseDto>> AddUserAsync(
        [FromBody] AddUserRequestDto request)
    {
        Console.WriteLine("Username" + request.Username + "   " +
                          request.Password);
        if (!await userRepository.GetUsers().AnyAsync(u=>u.Username==request.Username))
        {
            User user = Entities.User.GetUser();
            user.Username = request.Username;
            user.Password = request.Password;

            User created = await userRepository.AddUserAsync(user);

            AddUserResponseDto sendDto = new()
                { UserId = created.UserId, Username = created.Username };
            return Created($"/Users", sendDto);
        }

        return BadRequest("Username is invalid.");
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AddUserResponseDto>> GetUserAsync(
        [FromRoute] int id)
    {
        User user = await userRepository.GetUserByIdAsync(id);
        try
        {
            AddUserResponseDto sendDto = new AddUserResponseDto
                { UserId = user.UserId, Username = user.Username };
            return Created($"/Users/{id}", sendDto);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>>
        GetAllUsersAsync(
            [FromQuery] string? nameContains = null)
    {
        IList<User> users = await userRepository.GetUsers().Where(u =>
            nameContains == null || u.Username.ToLower().Trim()
                .Contains(nameContains.ToLower().Trim())).ToListAsync();
        
        return Ok(users);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AddUserResponseDto>> ReplaceUserAsync(
        [FromBody] ReplaceUserRequestDto request, [FromRoute] int id)
    {

        if (!await userRepository.GetUsers().AnyAsync(u=>u.Username==request.Username))
        {
            User user = Entities.User.GetUser();
            user.UserId = request.UserId;
            user.Username = request.Username;
            user.Password = request.Password;

            await userRepository.UpdateUserAsync(user);
            AddUserResponseDto sendDto = new()
                { UserId = user.UserId, Username = user.Username };
            return Ok(sendDto);
        }

        return BadRequest("Username is invalid.");
    }

    [HttpDelete("{id}")]
    public async Task<IResult> DeleteUserAsync([FromRoute] int id)
    {
        await userRepository.DeleteUserAsync(id);
        return Results.NoContent();
    }
}