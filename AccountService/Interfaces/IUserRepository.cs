public interface IUserRepository
{
    Task CreateAsync(User user);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(int id);
    Task<User> UpdateAsync(User user);
    Task ActivateUserAsync(int userId);
}