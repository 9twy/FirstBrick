public class UserCreatedEvent
{
    public int UserId { get; set; }
    public string? EventType { get; set; }
    public string? TargetService { get; set; }

}