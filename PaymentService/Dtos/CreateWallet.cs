using System.ComponentModel.DataAnnotations;

public record class CreateWalletDto(
    [Required] int UserId
);