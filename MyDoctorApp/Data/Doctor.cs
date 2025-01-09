using MyDoctorApp.Core.Enums;

namespace MyDoctorApp.Data
{
    public class Doctor : BaseEntity
    {
        public int Id { get; set; }
        public string Afm { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public DoctorSpecialty DoctorSpecialty { get; set; } 
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Patient> Patients { get; set; } = new HashSet<Patient>();    
    }
}
