using AccountService.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.IO.IsolatedStorage;
using System.Text.Json;
using static AccountService.Exceptions.AppException;
using AccountService.Extensions;
using RabbitMQ.Stream.Client;
using RabbitMQ.Stream.Client.Reliable;

public class UserService : IUserService
{
    private readonly IEventPublisher _eventPublisher;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;
    private readonly PasswordHasher<User> _passwordHasher;
    private readonly IConfiguration _configuration;
    public UserService(IUserRepository userRepository, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
        _passwordHasher = new PasswordHasher<User>();
        _configuration = configuration;

    }

    public async Task<UserResponseDto> RegisterUserAsync(RegisterUserDto registerUserDto)
    {
        var existingUser = await _userRepository.GetByEmailAsync(registerUserDto.Email);
        if (existingUser != null)
        {
            throw new EmailAlreadyExistsException("Email is already Exists.");
        }
        var user = new User
        {
            Email = registerUserDto.Email,
            FirstName = registerUserDto.FirstName,
            LastName = registerUserDto.LastName,
            PhoneNumber = registerUserDto.PhoneNumber,
        };
        user.PasswordHash = _passwordHasher.HashPassword(user, registerUserDto.Password);
        await _userRepository.CreateAsync(user);
        var streamSystem = await StreamSystem.Create(new StreamSystemConfig());
        var producer = await Producer.Create(new ProducerConfig(streamSystem, "wallet-stream"));

        // var @event = new UserCreatedEvent
        // {
        //     UserId = user.Id,
        //     EventType = "UserCreated",
        //     TargetService = "WalletService"
        // };
        // var messageBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));
        // await producer.Send(new Message(messageBytes));
        var userCreatedEvent = new UserCreatedEvent
        {
            UserId = user.Id,
            EventType = "UserCreated",
            TargetService = "WalletService"
        };

        await _eventPublisher.PublishAsync("wallet-stream", userCreatedEvent);
        return new UserResponseDto(
            user.Id,
            user.Email,
            $"{user.FirstName} {user.LastName}"
        );
    }

    public async Task<LoginResponseDto> LoginUserAsync(LoginDto loginDto)
    {
        var user = await _userRepository.GetByEmailAsync(loginDto.Email);
        if (user == null)
        {
            throw new NotFoundException("User not found.");
        }
        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, loginDto.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            throw new NotFoundException("Invalid password.");
        }
        return new LoginResponseDto(
            User: new UserResponseDto(
                user.Id,
                user.Email,
                $"{user.FirstName} {user.LastName}"
            ),
            Token: GenerateToken(user)
        );


    }
    private string GenerateToken(User user)
    {
        var secretKey = _configuration["Jwt:SecretKey"]!;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)

        };
        var token = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);


    }
    public async Task<UserResponseDto> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new NotFoundException("User not found.");
        }
        return new UserResponseDto(
            user.Id,
            user.Email,
            $"{user.FirstName} {user.LastName}"
        );
    }
    public async Task<UserResponseDto> UpdateUserAsync(int Id, UpdateUserDto updateUserDto)
    {
        var _user = _httpContextAccessor.HttpContext?.User;
        var user = await _userRepository.GetByIdAsync(Id);
        if (user == null)
        {
            throw new NotFoundException("Invalid User Id.");
        }
        if (_user!.GetUserId() != Id)
        {

            throw new UnauthorizedAccessException("You are not authorized to update this user.");
        }
        if (user.Email != updateUserDto.Email)
        {
            var existingUser = await _userRepository.GetByEmailAsync(updateUserDto.Email);
            if (existingUser != null)
            {
                throw new EmailAlreadyExistsException("Email is already Exists.");
            }

        }


        user.FirstName = updateUserDto.FirstName;
        user.LastName = updateUserDto.LastName;
        user.PhoneNumber = updateUserDto.PhoneNumber;
        user.Email = updateUserDto.Email;
        await _userRepository.UpdateAsync(user);
        return new UserResponseDto(
            user.Id,
            user.Email,
            $"{user.FirstName} {user.LastName}"
        );
    }
    public async Task ActivateUserAsync(int userId)
    {
        await _userRepository.ActivateUserAsync(userId);

    }

}