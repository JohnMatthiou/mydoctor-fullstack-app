using System.ComponentModel.DataAnnotations;

namespace MyDoctorApp.DTO
{
    public class UserLoginDTO
    {
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Username must be between 2 and 50 characters.")]
        public string? Username { get; set; }

        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?\d)(?=.*?\W).{8,20}$",
            ErrorMessage = "Password must contain at least one uppercase, one lowercase, " +
            "one digit, one special character and be between 8 to 20 characters")]
        public string? Password { get; set; }
    }
}

