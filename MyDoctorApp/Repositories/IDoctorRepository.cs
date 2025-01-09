using MyDoctorApp.Core.Filters;
using MyDoctorApp.Data;

namespace MyDoctorApp.Repositories
{
    public interface IDoctorRepository
    {
        Task<Doctor?> GetByAfmAsync(string afm);
        Task<Doctor?> GetByPhoneNumberAsync(string phoneNumber);
        Task<Doctor?> GetByUserIdAsync(int userId);
        Task<List<Patient>> GetDoctorPatientsPaginatedAsync(int userId, int pageNumber, int pageSize);
        Task<List<Doctor>> GetDoctorsFilteredPaginatedAsync(DoctorFilters filters, int pageNumber, int pageSize);
        Task<int> GetDoctorsFilteredCountAsync(DoctorFilters filters);
        Task<int> GetDoctorPatientsCountAsync(int userId);
    }
}
