using MyDoctorApp.Core.Filters;
using MyDoctorApp.DTO;

namespace MyDoctorApp.Services
{
    public interface IDoctorService
    {
        Task<UserReadOnlyDTO> SignUpUserAsync(DoctorSignupDTO request);
        Task RemovePatientFromDoctorAsync(int userIdDoctor, int userIdPatient);
        Task<UserDoctorReadOnlyDTO> GetByUserIdAsync(int id);
        Task<UserDoctorReadOnlyDTO> UpdateUserAsync(int id, UserUpdateDTO userUpdateDTO);
        Task<PaginatedResultDTO<UserPatientReadOnlyDTO>> GetDoctorPatientsPaginatedAsync(int userIdDoctor, int pageNumber, int pageSize);
        Task<PaginatedResultDTO<UserDoctorReadOnlyDTO>> GetDoctorsFilteredPaginatedAsync(DoctorFilters filters, int pageNumner, int pageSize);
    }
}
