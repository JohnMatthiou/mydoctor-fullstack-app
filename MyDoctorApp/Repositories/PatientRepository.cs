using Microsoft.EntityFrameworkCore;
using MyDoctorApp.Data;

namespace MyDoctorApp.Repositories
{
    public class PatientRepository : BaseRepository<Patient>, IPatientRepository
    {
        public PatientRepository(MyDoctorAppDbContext context) : base(context)
        {
        }

        public async Task<Patient?> GetByAmkaAsync(string amka) => await context.Patients.FirstOrDefaultAsync(p => p.Amka == amka);

        public async Task<Patient?> GetByUserIdAsync(int userId) => await context.Patients.Include(p => p.Doctors).Include(p => p.User).FirstOrDefaultAsync(p => p.UserId == userId);

        public async Task<List<Doctor>> GetPatientDoctorsAsync(int id)
        {
            return await context.Patients
                                 .Where(p => p.Id == id)
                                 .SelectMany(p => p.Doctors)
                                 .ToListAsync();
        }

        public async Task<int> GetPatientDoctorsCountAsync(int userId)
        {
            return await context.Patients
                .Where (p => p.UserId == userId)
                .SelectMany (p => p.Doctors)
                .CountAsync();
        }

        public async Task<List<Doctor>> GetPatientDoctorsPaginatedAsync(int userId, int pageNumber, int pageSize)
        {
            int skip = (pageNumber - 1) * pageSize;
            var patientDoctors = await context.Patients
                .Where(p => p.UserId == userId)
                .SelectMany(p => p.Doctors)
                .Include(d => d.User)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return patientDoctors;
        }
    }
}
