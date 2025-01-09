namespace MyDoctorApp.Repositories
{
    public interface IUnitOfWork
    {
        UserRepository UserRepository { get; }
        PatientRepository PatientRepository { get; }
        DoctorRepository DoctorRepository { get; }

        Task<bool> SaveAsync();
    }
}
