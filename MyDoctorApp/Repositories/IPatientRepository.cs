using MyDoctorApp.Data;

namespace MyDoctorApp.Repositories
{
    public interface IPatientRepository
    {
        Task<Patient?> GetByAmkaAsync(string amka);
        Task<Patient?> GetByUserIdAsync(int userId);
        Task<List<Doctor>> GetPatientDoctorsAsync(int id);
        Task<List<Doctor>> GetPatientDoctorsPaginatedAsync(int userId, int pageNumber, int pageSize);
        Task<int> GetPatientDoctorsCountAsync(int userId);
    }
}
