using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

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
    public async Task<ActionResult<AddUserResponseDto>> AddUser(
        [FromBody] AddUserRequestDto request)
    {
        if (await userRepository.IsUsernameValidAsync(request.Username))
        {
            User user = new User
            {
                Username = request.Username, Password = request.Password
            };
            User created = await userRepository.AddUserAsync(user);

            AddUserResponseDto dtoSend = new()
                { UserId = created.UserId, Username = created.Username };

            return Created($"/Users/{dtoSend.UserId}", created);
        }

        return BadRequest("Username is invalid.");
    }

}