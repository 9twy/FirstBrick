public class Project
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Location { get; set; }
    public required int NumberOfUnits { get; set; }
    public int UintsFilled { get; set; } = 0;
    public int UnitPrice { get; set; }
    public ICollection<Investment> Investments { get; set; } = new List<Investment>();
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }


}