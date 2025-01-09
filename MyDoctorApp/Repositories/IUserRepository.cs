using MyDoctorApp.Data;

namespace MyDoctorApp.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserAsync(string username, string password);
        Task<User?> UpdateUserAsync(int id, User user);
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
    }
}
