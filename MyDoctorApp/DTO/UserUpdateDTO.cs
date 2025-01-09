using MyDoctorApp.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace MyDoctorApp.DTO
{
    public class UserUpdateDTO
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Username must be between 2 and 50 characters.")]
        [RegularExpression(@"^\S+$", ErrorMessage = "The {0} cannot contain only whitespaces.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(100, ErrorMessage = "Email must not exceed 100 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }


        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Firstname must be between 2 and 50 characters.")]
        [RegularExpression(@"^\S+$", ErrorMessage = "The {0} cannot contain only whitespaces.")]
        public string? Firstname { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Lastname must be between 2 and 50 characters.")]
        [RegularExpression(@"^\S+$", ErrorMessage = "The {0} cannot contain only whitespaces.")]
        public string? Lastname { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Phone number must be at least 10 characters and not exceed 15 characters.")]
        [RegularExpression(@"^\S+$", ErrorMessage = "The {0} cannot contain only whitespaces.")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "City must be between 2 and 50 characters.")]
        [RegularExpression(@"^\S+$", ErrorMessage = "The {0} cannot contain only whitespaces.")]
        public string? City { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Address must be between 2 and 50 characters.")]
        [RegularExpression(@"^[^\s].*[^\s]$", ErrorMessage = "The {0} cannot contain only whitespaces.")]
        public string? Address { get; set; }

    }
}
