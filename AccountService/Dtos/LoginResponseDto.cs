public record class LoginResponseDto(
    UserResponseDto User,
    string Token
);