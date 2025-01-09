using MyDoctorApp.Data;

namespace MyDoctorApp.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyDoctorAppDbContext _context;

        public UnitOfWork(MyDoctorAppDbContext context)
        {
            _context = context;
        }

        public UserRepository UserRepository => new(_context);

        public PatientRepository PatientRepository => new(_context);

        public DoctorRepository DoctorRepository => new(_context);

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
