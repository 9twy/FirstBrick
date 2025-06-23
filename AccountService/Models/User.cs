
public class User
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public required String FirstName { get; set; }
    public required String LastName { get; set; }
    public required String PhoneNumber { get; set; }
    public Status Status { get; set; } = Status.UnActive;

    public string? PasswordHash { get; set; }

}
public enum Status
{
    UnActive = 0,
    Active = 1
}