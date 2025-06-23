using AccountService.Dtos;
public interface IUserService
{
    Task<UserResponseDto> RegisterUserAsync(RegisterUserDto dto);
    Task<LoginResponseDto> LoginUserAsync(LoginDto dto);
    Task<UserResponseDto> GetUserByIdAsync(int id);
    Task<UserResponseDto> UpdateUserAsync(int id, UpdateUserDto dto);
    Task ActivateUserAsync(int userId);
}