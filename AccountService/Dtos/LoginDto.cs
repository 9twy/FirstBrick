using System.ComponentModel.DataAnnotations;
namespace AccountService.Dtos;

public record class LoginDto(
    [Required][EmailAddress] string Email,
    [Required][MinLength(8)][MaxLength(100)][DataType(DataType.Password)] string Password
);