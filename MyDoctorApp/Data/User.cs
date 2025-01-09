using MyDoctorApp.Core.Enums;

namespace MyDoctorApp.Data
{
    public class User : BaseEntity
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public UserRole UserRole { get; set; }
        public virtual Patient? Patient { get; set; }
        public virtual Doctor? Doctor { get; set; }

    }
}
