using MyDoctorApp.Data;
using MyDoctorApp.DTO;

namespace MyDoctorApp.Services
{
    public interface IUserService
    {
        Task<User?> VerifyAndGetUserAsync(UserLoginDTO credentials);
        Task<User?> GetUserByIdAsync(int id);
    }
}
