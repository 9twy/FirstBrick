public class Investment
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public required virtual Project Project { get; set; }
    public required int Units { get; set; }
    public int amount { get; set; }
    public DateOnly InvestedAt { get; set; }


}