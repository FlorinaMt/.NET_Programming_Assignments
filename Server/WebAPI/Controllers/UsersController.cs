using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
        Console.WriteLine("Username" + request.Username + "   " + request.Password);
        if (await userRepository.IsUsernameValidAsync(request.Username))
        {
            User user = new User
            {
                Username = request.Username, Password = request.Password
            };
            User created = await userRepository.AddUserAsync(user);

            AddUserResponseDto dtoSend = new()
                { UserId = created.UserId, Username = created.Username };
            return Created($"/Users", dtoSend);

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
                { UserId = id, Username = user.Username };
            return Created($"/Users/{id}", sendDto);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public async Task<IResult> GetAllUsersAsync(
        [FromQuery] string? nameContains)
    {
        List<string> usernames = new List<string>();

        for (int i = 0; i < userRepository.GetUsers().Count(); i++)
            usernames.Add(userRepository.GetUsers().ElementAt(i).Username);

        if (!string.IsNullOrWhiteSpace(nameContains))
            usernames = usernames
                .Where(u => u.ToLower().Contains(nameContains.Trim().ToLower()))
                .ToList();

        if (usernames.Count == 0)
            return Results.Ok("No users found.");
        return Results.Ok(usernames);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AddUserResponseDto>> ReplaceUserAsync(
        [FromBody] ReplaceUserRequestDto request, [FromRoute] int id)
    {
        //check the username and password together? 

        if (await userRepository.IsUsernameValidAsync(request.Username))
        {
            User user = new User
            {
                UserId = request.UserId, Username = request.Username,
                Password = request.Password
            };
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
        //check for the username and password together

        await userRepository.DeleteUserAsync(id);
        return Results.NoContent();
    }
}