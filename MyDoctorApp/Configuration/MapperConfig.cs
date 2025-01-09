using AutoMapper;
using MyDoctorApp.Core.Filters;
using MyDoctorApp.Data;
using MyDoctorApp.DTO;


namespace MyDoctorApp.Configuration
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {

            
            CreateMap<User, UserReadOnlyDTO>().ReverseMap();

            CreateMap<User, PatientSignupDTO>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => $"{src.Username}"))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => $"{src.Email}"))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => $"{src.Password}"))
                .ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => $"{src.Firstname}"))
                .ForMember(dest => dest.Lastname, opt => opt.MapFrom(src => $"{src.Lastname}"))
                .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => $"{src.UserRole}"))
                .ReverseMap();

            CreateMap<User, DoctorSignupDTO>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => $"{src.Username}"))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => $"{src.Email}"))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => $"{src.Password}"))
                .ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => $"{src.Firstname}"))
                .ForMember(dest => dest.Lastname, opt => opt.MapFrom(src => $"{src.Lastname}"))
                .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => $"{src.UserRole}"))
                .ReverseMap();

            CreateMap<Patient, PatientSignupDTO>()
                .ForMember(dest => dest.Amka, opt => opt.MapFrom(src => $"{src.Amka}"))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => $"{src.City}"))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address}"))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => $"{src.PhoneNumber}"))
                .ReverseMap();

            CreateMap<Doctor, DoctorSignupDTO>()
                .ForMember(dest => dest.Afm, opt => opt.MapFrom(src => $"{src.Afm}"))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => $"{src.City}"))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address}"))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => $"{src.PhoneNumber}"))
                .ForMember(dest => dest.DoctorSpecialty, opt => opt.MapFrom(src => $"{src.DoctorSpecialty}"))
                .ReverseMap();

            CreateMap<Patient, UserPatientReadOnlyDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => $"{src.User!.Id}"))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => $"{src.User!.Username}"))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => $"{src.User!.Email}"))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => $"{src.User!.Password}"))
                .ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => $"{src.User!.Firstname}"))
                .ForMember(dest => dest.Lastname, opt => opt.MapFrom(src => $"{src.User!.Lastname}"))
                .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => $"{src.User!.UserRole}"))
                .ForMember(dest => dest.Amka, opt => opt.MapFrom(src => $"{src.Amka}"))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => $"{src.City}"))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address}"))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => $"{src.PhoneNumber}"))
                .ReverseMap();

            CreateMap<User, UserPatientReadOnlyDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => $"{src.Id}"))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => $"{src.Username}"))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => $"{src.Email}"))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => $"{src.Password}"))
                .ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => $"{src.Firstname}"))
                .ForMember(dest => dest.Lastname, opt => opt.MapFrom(src => $"{src.Lastname}"))
                .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => $"{src.UserRole}"))
                .ForMember(dest => dest.Amka, opt => opt.MapFrom(src => $"{src.Patient!.Amka}"))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => $"{src.Patient!.City}"))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Patient!.Address}"))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => $"{src.Patient!.PhoneNumber}"))
                .ReverseMap();

            CreateMap<Doctor, UserDoctorReadOnlyDTO>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => $"{src.User!.Id}"))
               .ForMember(dest => dest.Username, opt => opt.MapFrom(src => $"{src.User!.Username}"))
               .ForMember(dest => dest.Email, opt => opt.MapFrom(src => $"{src.User!.Email}"))
               .ForMember(dest => dest.Password, opt => opt.MapFrom(src => $"{src.User!.Password}"))
               .ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => $"{src.User!.Firstname}"))
               .ForMember(dest => dest.Lastname, opt => opt.MapFrom(src => $"{src.User!.Lastname}"))
               .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => $"{src.User!.UserRole}"))
               .ForMember(dest => dest.Afm, opt => opt.MapFrom(src => $"{src.Afm}"))
               .ForMember(dest => dest.City, opt => opt.MapFrom(src => $"{src.City}"))
               .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address}"))
               .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => $"{src.PhoneNumber}"))
               .ForMember(dest => dest.DoctorSpecialty, opt => opt.MapFrom(src => $"{src.DoctorSpecialty}"))
               .ReverseMap();

            CreateMap<User, UserDoctorReadOnlyDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => $"{src.Id}"))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => $"{src.Username}"))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => $"{src.Email}"))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => $"{src.Password}"))
                .ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => $"{src.Firstname}"))
                .ForMember(dest => dest.Lastname, opt => opt.MapFrom(src => $"{src.Lastname}"))
                .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => $"{src.UserRole}"))
                .ForMember(dest => dest.Afm, opt => opt.MapFrom(src => $"{src.Doctor!.Afm}"))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => $"{src.Doctor!.City}"))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Doctor!.Address}"))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => $"{src.Doctor!.PhoneNumber}"))
                .ForMember(dest => dest.DoctorSpecialty, opt => opt.MapFrom(src => $"{src.Doctor!.DoctorSpecialty}"))
                .ReverseMap();

            CreateMap<PaginatedResult<Doctor>, PaginatedResultDTO<UserDoctorReadOnlyDTO>>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data))  
                .ForMember(dest => dest.TotalRecords, opt => opt.MapFrom(src => src.TotalRecords))
                .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.PageNumber))
                .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize))
                .ForMember(dest => dest.TotalPages, opt => opt.MapFrom(src => src.TotalPages));

            CreateMap<PaginatedResult<Patient>, PaginatedResultDTO<UserPatientReadOnlyDTO>>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data))
                .ForMember(dest => dest.TotalRecords, opt => opt.MapFrom(src => src.TotalRecords))
                .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.PageNumber))
                .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize))
                .ForMember(dest => dest.TotalPages, opt => opt.MapFrom(src => src.TotalPages));
        }
    }
}
