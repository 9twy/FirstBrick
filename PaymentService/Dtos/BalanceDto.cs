using System.ComponentModel.DataAnnotations;

public record class BalnaceDto(
  [Required] decimal balance
);