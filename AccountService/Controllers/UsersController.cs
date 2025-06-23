using Microsoft.AspNetCore.Mvc;
using AccountService.Dtos;
using Microsoft.AspNetCore.Authorization;
using AccountService.Extensions;
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> logger;


    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        this.logger = logger;
        _userService = userService;
    }




    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto dto)
    {
        var responseDto = await _userService.RegisterUserAsync(dto);
        return CreatedAtRoute(routeValues: new { id = responseDto.Id }, value: responseDto);
    }

    [HttpPost("login")]
    public async Task<IActionResult> login(LoginDto loginDto)
    {
        var loginResponseDto = await _userService.LoginUserAsync(loginDto);
        return CreatedAtRoute(routeValues: new { id = loginDto.Email }, value: loginResponseDto);
    }


    [HttpGet("{user_id}")]
    [Authorize]
    public async Task<IActionResult> getProfile(int user_id)
    {
        var user = await _userService.GetUserByIdAsync(user_id);
        return Ok(user);
    }

    [HttpPut("{user_id}")]
    [Authorize]
    public async Task<IActionResult> UpdateProfile(int user_id, UpdateUserDto updateUserDto)
    {
        var responseDto = await _userService.UpdateUserAsync(user_id, updateUserDto);
        return NoContent();
    }
}