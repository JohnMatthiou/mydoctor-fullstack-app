using AutoMapper;
using MyDoctorApp.Core.Filters;
using MyDoctorApp.Data;
using MyDoctorApp.DTO;
using MyDoctorApp.Exceptions;
using MyDoctorApp.Repositories;
using MyDoctorApp.Security;
using Serilog;
using System.Numerics;

namespace MyDoctorApp.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<DoctorService> _logger;

        public DoctorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = new LoggerFactory().AddSerilog().CreateLogger<DoctorService>();
        }

        public async Task<UserDoctorReadOnlyDTO> GetByUserIdAsync(int id)
        {
            Doctor? doctor;

            try
            {
                doctor = await _unitOfWork.DoctorRepository.GetByUserIdAsync(id);
                if (doctor == null)
                {
                    throw new EntityNotFoundException("Doctor", "Doctor with user id " + id + " wasn't found");
                }

                return _mapper.Map<UserDoctorReadOnlyDTO>(doctor);
            }
            catch (Exception ex)
            {
                _logger.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async Task<PaginatedResultDTO<UserPatientReadOnlyDTO>> GetDoctorPatientsPaginatedAsync(int userIdDoctor, int pageNumber, int pageSize)
        {
            List<Patient> doctorPatients = new();
            int totalPatientsCount;
            PaginatedResult<Patient> pageToReturn;
            try
            {
                if (await _unitOfWork.DoctorRepository.GetByUserIdAsync(userIdDoctor) == null)
                {
                    throw new EntityNotFoundException("Doctor", "Doctor with user id " + userIdDoctor + " wasn't found");
                }

                totalPatientsCount = await _unitOfWork.DoctorRepository.GetDoctorPatientsCountAsync(userIdDoctor);
                if (totalPatientsCount == 0)
                {
                    throw new EntityNotFoundException("Patients", "No patients found for doctor with user id " + userIdDoctor);
                }

                doctorPatients = await _unitOfWork.DoctorRepository.GetDoctorPatientsPaginatedAsync(userIdDoctor, pageNumber, pageSize);

                pageToReturn = new PaginatedResult<Patient>
                {
                    Data = doctorPatients,
                    TotalRecords = totalPatientsCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                return _mapper.Map<PaginatedResultDTO<UserPatientReadOnlyDTO>>(pageToReturn);

            }
            catch (Exception ex)
            {
                _logger.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async Task<PaginatedResultDTO<UserDoctorReadOnlyDTO>> GetDoctorsFilteredPaginatedAsync(DoctorFilters filters, int pageNumber, int pageSize)
        {
            List<Doctor> filteredDoctors = new();
            int totalDoctorsCount;
            PaginatedResult<Doctor> pageToReturn;
            try
            {
                totalDoctorsCount = await _unitOfWork.DoctorRepository.GetDoctorsFilteredCountAsync(filters);
                if (totalDoctorsCount == 0)
                {
                    throw new EntityNotFoundException("Doctors", "No doctors found matching the given criteria");
                }

                filteredDoctors = await _unitOfWork.DoctorRepository.GetDoctorsFilteredPaginatedAsync(filters, pageNumber, pageSize);              

                pageToReturn = new PaginatedResult<Doctor>
                {
                    Data = filteredDoctors,
                    TotalRecords = totalDoctorsCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                return _mapper.Map<PaginatedResultDTO<UserDoctorReadOnlyDTO>>(pageToReturn);

            }
            catch (Exception ex)
            {
                _logger.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async Task RemovePatientFromDoctorAsync(int userIdDoctor, int userIdPatient)
        {
            Patient? patient;
            Doctor? doctor;

            try
            {
                doctor = await _unitOfWork.DoctorRepository.GetByUserIdAsync(userIdDoctor);
                if (doctor == null)
                {
                    throw new EntityNotFoundException("Doctor", "Doctor with user id " + userIdDoctor + " wasn't found");
                }

                patient = await _unitOfWork.PatientRepository.GetByUserIdAsync(userIdPatient);
                if (patient == null)
                {
                    throw new EntityNotFoundException("Patient", "Patient with user id " + userIdPatient + " wasn't found");
                }

                var patientToRemove = doctor.Patients.FirstOrDefault(p => p.Id == patient.Id);
                if (patientToRemove == null)
                {
                    throw new EntityNotFoundException("Patient", "Patient with user id " + userIdPatient + " is not associated with the doctor");
                }

                doctor.Patients.Remove(patientToRemove);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("{Message}", "Patient: " + patientToRemove + " removed from doctor with user id " + userIdDoctor);
            }
            catch (Exception ex)
            {
                _logger.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async Task<UserReadOnlyDTO> SignUpUserAsync(DoctorSignupDTO request)
        {
            Doctor doctor;
            User user;

            try
            {
                user = _mapper.Map<User>(request);
                User? existingUserByUsername = await _unitOfWork.UserRepository.GetByUsernameAsync(user.Username);
                if (existingUserByUsername != null)
                {
                    throw new EntityAlreadyExistsException("User", "User with username " +
                        existingUserByUsername.Username + " already exists");
                }

                User? existingUserByEmail = await _unitOfWork.UserRepository.GetByEmailAsync(user.Email);
                if (existingUserByEmail != null)
                {
                    throw new EntityAlreadyExistsException("User", "User with email " + existingUserByEmail.Email + " already exists");
                }

                user.Password = EncryptionUtil.Encrypt(user.Password);
                await _unitOfWork.UserRepository.AddAsync(user);

                doctor = _mapper.Map<Doctor>(request);
                Doctor? existingDoctorByAfm = await _unitOfWork.DoctorRepository.GetByAfmAsync(doctor.Afm);
                if (existingDoctorByAfm != null)
                {
                    throw new EntityAlreadyExistsException("Doctor", "Doctor with afm " + existingDoctorByAfm.Afm + " already exists");
                }

                Doctor? existingDoctorByPhoneNumber = await _unitOfWork.DoctorRepository.GetByPhoneNumberAsync(doctor.PhoneNumber);
                if (existingDoctorByPhoneNumber != null)
                {
                    throw new EntityAlreadyExistsException("Doctor", "Doctor with phone number " + existingDoctorByPhoneNumber.PhoneNumber + " already exists");
                }

                await _unitOfWork.DoctorRepository.AddAsync(doctor);
                user.Doctor = doctor;

                await _unitOfWork.SaveAsync();
                _logger.LogInformation("{Message}", "Doctor: " + doctor + " signed up successfully.");
                return _mapper.Map<UserReadOnlyDTO>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async Task<UserDoctorReadOnlyDTO> UpdateUserAsync(int id, UserUpdateDTO userUpdateDTO)
        {
            User? userDoctor;

            try
            {
                if (id != userUpdateDTO.Id)
                {
                    throw new InvalidArgumentException("ID", "The Id in the route does not match the Id in the body.");
                }

                userDoctor = await _unitOfWork.UserRepository.GetByIdAsync(id);
                if (userDoctor == null)
                {
                    throw new EntityNotFoundException("Doctor", "Doctor with user id " + id + " wasn't found.");
                }

                if (userDoctor.Doctor == null)
                {
                    throw new EntityNotFoundException("Doctor", "Doctor for user with id " + id + " wasn't found.");
                }

                User? existingUserByUsername = await _unitOfWork.UserRepository.GetByUsernameAsync(userUpdateDTO.Username!);
                if (existingUserByUsername != null && existingUserByUsername.Id != id)
                {
                    throw new EntityAlreadyExistsException("User", "User with username " +
                        existingUserByUsername.Username + " already exists");
                }

                User? existingUserByEmail = await _unitOfWork.UserRepository.GetByEmailAsync(userUpdateDTO.Email!);
                if (existingUserByEmail != null && existingUserByEmail.Id != id)
                {
                    throw new EntityAlreadyExistsException("User", "User with email " + existingUserByEmail.Email + " already exists");
                }

                Doctor? existingDoctorByPhoneNumber = await _unitOfWork.DoctorRepository.GetByPhoneNumberAsync(userUpdateDTO.PhoneNumber!);
                if (existingDoctorByPhoneNumber != null && existingDoctorByPhoneNumber.UserId != id)
                {
                    throw new EntityAlreadyExistsException("Doctor", "Doctor with phone number " + existingDoctorByPhoneNumber.PhoneNumber + " already exists");
                }

                userDoctor.Username = userUpdateDTO.Username!;
                userDoctor.Email = userUpdateDTO.Email!;
                userDoctor.Firstname = userUpdateDTO.Firstname!;
                userDoctor.Lastname = userUpdateDTO.Lastname!;
                userDoctor.Doctor.City = userUpdateDTO.City!;
                userDoctor.Doctor.Address = userUpdateDTO.Address!;
                userDoctor.Doctor.PhoneNumber = userUpdateDTO.PhoneNumber!;

                await _unitOfWork.UserRepository.UpdateAsync(userDoctor);
                await _unitOfWork.SaveAsync();
                _logger.LogInformation("{Message}", "Doctor with user id " + id + " was updated successfully.");
                return _mapper.Map<UserDoctorReadOnlyDTO>(userDoctor);

            }
            catch (Exception ex)
            {
                _logger.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }
        }
    }
}
