public record class ProjectDto
(
    int Id,
    int UserId,
    string Name,
    string Description,
    string Location,
    int NumberOfUnits,
    int UintsFilled,
    int UnitPrice,
    DateOnly StartDate,
    DateOnly EndDate
);