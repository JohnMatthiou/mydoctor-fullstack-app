namespace MyDoctorApp.Data
{
    public class Patient : BaseEntity
    {
        public int Id { get; set; }
        public string Amka { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;
        public ICollection<Doctor> Doctors { get; set; } = new HashSet<Doctor>();

    }
}
