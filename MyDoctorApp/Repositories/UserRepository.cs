using Microsoft.EntityFrameworkCore;
using MyDoctorApp.Data;
using MyDoctorApp.Security;

namespace MyDoctorApp.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(MyDoctorAppDbContext context) : base(context)
        {
        }

        public async Task<User?> GetUserAsync(string username, string password)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return null;
            }
            if (!EncryptionUtil.IsValidPassword(password, user.Password!))
            {
                return null;
            }
            return user;
        }

        public async Task<User?> GetByIdAsync(int id) =>
            await context.Users.Include(u => u.Patient).Include(u => u.Doctor).FirstOrDefaultAsync(u => u.Id == id);

        public async Task<User?> GetByEmailAsync(string email) =>
            await context.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<User?> GetByUsernameAsync(string username) =>
            await context.Users.FirstOrDefaultAsync(u => u.Username == username);

        public async Task<User?> UpdateUserAsync(int id, User user)
        {
            var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (existingUser is null) return null;
            if (existingUser.Id != id) return null;

            context.Users.Attach(user);
            context.Entry(user).State = EntityState.Modified;
            return existingUser;
        }

    }
}
