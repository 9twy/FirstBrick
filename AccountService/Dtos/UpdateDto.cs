using System.ComponentModel.DataAnnotations;

public record class UpdateUserDto(
    [MinLength(2), MaxLength(50)]
    string FirstName,
    [MinLength(2), MaxLength(50)]
    string LastName,
    [Phone]
    [Length(10,10)]
    string PhoneNumber,
    [EmailAddress]
    string Email
);