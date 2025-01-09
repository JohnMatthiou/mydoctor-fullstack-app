using MyDoctorApp.Core.Enums;

namespace MyDoctorApp.Core.Filters
{
    public class DoctorFilters
    {
        public string? Lastname { get; set; }
        public string? City { get; set; }
        public DoctorSpecialty? Specialty { get; set; }

    }
}
