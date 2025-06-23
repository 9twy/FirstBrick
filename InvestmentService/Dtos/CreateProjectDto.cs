using System.ComponentModel.DataAnnotations;

public record class CreateProjectDto(
    [Required, MaxLength(100)] string Name,
    [Required, MaxLength(500)] string Description,
    [Required, MaxLength(200)] string Location,
    [Required, Range(1, 1000)] int NumberOfUnits,
    [Required, Range(1, 1000000)] int UnitPrice,
    [Required] DateOnly StartDate,
    [Required] DateOnly EndDate
);
