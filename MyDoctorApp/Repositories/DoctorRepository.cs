using Microsoft.EntityFrameworkCore;
using MyDoctorApp.Core.Filters;
using MyDoctorApp.Data;

namespace MyDoctorApp.Repositories
{
    public class DoctorRepository : BaseRepository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(MyDoctorAppDbContext context) : base(context)
        {
        }

        public async Task<Doctor?> GetByAfmAsync(string afm) =>
            await context.Doctors.FirstOrDefaultAsync(d => d.Afm == afm);


        public async Task<Doctor?> GetByPhoneNumberAsync(string phoneNumber) =>
            await context.Doctors.FirstOrDefaultAsync(d => d.PhoneNumber == phoneNumber);

        public async Task<Doctor?> GetByUserIdAsync(int userId) => await context.Doctors.Include(d => d.Patients).Include(d => d.User).FirstOrDefaultAsync(d => d.UserId == userId);

        public async Task<int> GetDoctorPatientsCountAsync(int userId)
        {
            return await context.Doctors
                .Where(d => d.UserId == userId)
                .SelectMany(d => d.Patients)
                .CountAsync();
        }

        public async Task<List<Patient>> GetDoctorPatientsPaginatedAsync(int userId, int pageNumber, int pageSize)
        {
            int skip = (pageNumber - 1) * pageSize;
            var doctorPatients = await context.Doctors
                .Where(d => d.UserId == userId)
                .SelectMany(d => d.Patients)
                .Include(p => p.User)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return doctorPatients;
        }

        public async Task<int> GetDoctorsFilteredCountAsync(DoctorFilters filters)
        {
            int totalRecords = 0;

            var query = context.Doctors.Include(d => d.User).AsQueryable(); ;

            if (!string.IsNullOrEmpty(filters.Lastname))
            {
                query = query.Where(d => d.User.Lastname.StartsWith(filters.Lastname));
            }

            if (!string.IsNullOrEmpty(filters.City))
            {
                query = query.Where(d => d.City.StartsWith(filters.City));
            }

            if (filters.Specialty.HasValue)
            {
                query = query.Where(d => d.DoctorSpecialty == filters.Specialty.Value);
            }

            totalRecords = await query.CountAsync();
            return totalRecords;
        }

        public async Task<List<Doctor>> GetDoctorsFilteredPaginatedAsync(DoctorFilters filters, int pageNumber, int pageSize)
        {
            var query = context.Doctors.Include(d => d.User).AsQueryable(); ;

            if (!string.IsNullOrEmpty(filters.Lastname))
            {
                query = query.Where(d => d.User.Lastname.StartsWith(filters.Lastname));
            }

            if (!string.IsNullOrEmpty(filters.City))
            {
                query = query.Where(d => d.City.StartsWith(filters.City));
            }

            if (filters.Specialty.HasValue)
            {
                query = query.Where(d => d.DoctorSpecialty == filters.Specialty.Value);
            }

            int skip = (pageNumber - 1) * pageSize;
            var filteredDoctors = await query
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return filteredDoctors;

        }
    }
}
