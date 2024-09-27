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
        public async Task<ActionResult<UserDto>> AddUser(
            [FromBody] CreateUserDto request)
        {
            try
            {
                User user = new(request.Username, request.Password);
                User created = await userRepository.AddUserAsync(user);
                UserDto dto = new()
                {
                    Id = created.Id,
                    Username = created.Username
                };
                return Created($"/Users/{dto.Id}", created);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
