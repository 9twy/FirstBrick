using System.ComponentModel.DataAnnotations;
namespace AccountService.Dtos;

public record class RegisterUserDto
(
[Required][EmailAddress] string Email,

[Required]
[MinLength(2)]
[MaxLength(50)]
string FirstName,
[Required]
[MinLength(2)]
[MaxLength(50)]
string LastName,
[Required]
[Phone]
[Length(10, 10)]
string PhoneNumber,

[Required]
[MinLength(8)]
[MaxLength(100)]
[DataType(DataType.Password)]
[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one digit.")]
string Password
);