﻿using MyDoctorApp.Core.Enums;

namespace MyDoctorApp.DTO
{
    public class UserDoctorReadOnlyDTO
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public UserRole? UserRole { get; set; }
        public string? Afm { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public DoctorSpecialty? DoctorSpecialty { get; set; }
    }
}