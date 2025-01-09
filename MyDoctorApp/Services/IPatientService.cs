using MyDoctorApp.DTO;

namespace MyDoctorApp.Services
{
    public interface IPatientService
    {
        Task<UserReadOnlyDTO> SignUpUserAsync(PatientSignupDTO request);
        Task AddDoctorToPatientAsync(int userIdPatient, int userIdDoctor);
        Task RemoveDoctorFromPatientAsync(int userIdPatient, int userIdDoctor);
        Task<UserPatientReadOnlyDTO> GetByUserIdAsync(int id);
        Task<UserPatientReadOnlyDTO> UpdateUserAsync(int id, UserUpdateDTO userUpdateDTO);
        Task<PaginatedResultDTO<UserDoctorReadOnlyDTO>> GetPatientDoctorsPaginatedAsync(int userIdPatient, int pageNumber, int pageSize);
    }
}
