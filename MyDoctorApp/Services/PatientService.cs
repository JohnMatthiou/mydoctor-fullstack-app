using AutoMapper;
using MyDoctorApp.Core.Filters;
using MyDoctorApp.Data;
using MyDoctorApp.DTO;
using MyDoctorApp.Exceptions;
using MyDoctorApp.Repositories;
using MyDoctorApp.Security;
using Serilog;

namespace MyDoctorApp.Services
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PatientService> _logger;

        public PatientService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = new LoggerFactory().AddSerilog().CreateLogger<PatientService>();
        }

        public async Task AddDoctorToPatientAsync(int userIdPatient, int userIdDoctor)
        {
            Patient? patient;
            Doctor? doctor;

            try
            {
                patient = await _unitOfWork.PatientRepository.GetByUserIdAsync(userIdPatient);
                if (patient == null)
                {
                    throw new EntityNotFoundException("Patient", "Patient with user id " + userIdPatient + " wasn't found");
                }

                doctor = await _unitOfWork.DoctorRepository.GetByUserIdAsync(userIdDoctor);
                if (doctor == null)
                {
                    throw new EntityNotFoundException("Doctor", "Doctor with user id " + userIdDoctor + " wasn't found");
                }

                var patientDoctors = await _unitOfWork.PatientRepository.GetPatientDoctorsAsync(patient.Id);
                if (patientDoctors.Any(d => d.Id == doctor.Id))
                {
                    throw new EntityAlreadyExistsException("Doctor", "Doctor with user id " + userIdDoctor + " already added to my doctors");
                }

                patient.Doctors.Add(doctor);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("{Message}", "Doctor: " + doctor + " added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async Task<UserPatientReadOnlyDTO> GetByUserIdAsync(int id)
        {
            Patient? patient;

            try
            {
                patient = await _unitOfWork.PatientRepository.GetByUserIdAsync(id);
                if (patient == null)
                {
                    throw new EntityNotFoundException("Patient", "Patient with user id " + id + " wasn't found");
                }

                return _mapper.Map<UserPatientReadOnlyDTO>(patient);
            }
            catch (Exception ex)
            {
                _logger.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }

        }


        public async Task<PaginatedResultDTO<UserDoctorReadOnlyDTO>> GetPatientDoctorsPaginatedAsync(int userIdPatient, int pageNumber, int pageSize)
        {
            List<Doctor> patientDoctors = new();
            int totalDoctorsCount;
            PaginatedResult<Doctor> pageToReturn;
            try
            {
                if (await _unitOfWork.PatientRepository.GetByUserIdAsync(userIdPatient) == null)
                {
                    throw new EntityNotFoundException("Patient", "Patient with user id " + userIdPatient + " wasn't found");
                }

                totalDoctorsCount = await _unitOfWork.PatientRepository.GetPatientDoctorsCountAsync(userIdPatient);
                if (totalDoctorsCount == 0)
                {
                    throw new EntityNotFoundException("Doctors", "No doctors found for patient with user id " + userIdPatient);
                }

                patientDoctors = await _unitOfWork.PatientRepository.GetPatientDoctorsPaginatedAsync(userIdPatient, pageNumber, pageSize);

                pageToReturn = new PaginatedResult<Doctor>
                {
                    Data = patientDoctors,
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

        public async Task RemoveDoctorFromPatientAsync(int userIdPatient, int userIdDoctor)
        {
            Patient? patient;
            Doctor? doctor;

            try
            {
                patient = await _unitOfWork.PatientRepository.GetByUserIdAsync(userIdPatient);
                if (patient == null)
                {
                    throw new EntityNotFoundException("Patient", "Patient with user id " + userIdPatient + " wasn't found");
                }

                doctor = await _unitOfWork.DoctorRepository.GetByUserIdAsync(userIdDoctor);
                if (doctor == null)
                {
                    throw new EntityNotFoundException("Doctor", "Doctor with user id " + userIdDoctor + " wasn't found");
                }

                var doctorToRemove = patient.Doctors.FirstOrDefault(d => d.Id == doctor.Id);
                if (doctorToRemove == null)
                {
                    throw new EntityNotFoundException("Doctor", "Doctor with user id " + userIdDoctor + " is not associated with the patient");
                }

                patient.Doctors.Remove(doctorToRemove);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("{Message}", "Doctor: " + doctorToRemove + " removed from patient with user id " + userIdPatient);
            }
            catch (Exception ex)
            {
                _logger.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async Task<UserReadOnlyDTO> SignUpUserAsync(PatientSignupDTO request)
        {
            Patient patient;
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

                patient = _mapper.Map<Patient>(request);
                Patient? existingPatientByAmka = await _unitOfWork.PatientRepository.GetByAmkaAsync(patient.Amka);
                if (existingPatientByAmka != null)
                {
                    throw new EntityAlreadyExistsException("Patient", "Patient with amka " + existingPatientByAmka.Amka + " already exists");
                }

                await _unitOfWork.PatientRepository.AddAsync(patient);
                user.Patient = patient;

                await _unitOfWork.SaveAsync();
                _logger.LogInformation("{Message}", "Patient: " + patient + " signed up successfully.");
                return _mapper.Map<UserReadOnlyDTO>(user); 
            }
            catch (Exception ex)
            {
                _logger.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }

        }

        public async Task<UserPatientReadOnlyDTO> UpdateUserAsync(int id, UserUpdateDTO userUpdateDTO)
        {
            User? userPatient;

            try
            {
                if (id != userUpdateDTO.Id)
                {
                    throw new InvalidArgumentException("ID", "The Id in the route does not match the Id in the body.");
                }

                userPatient = await _unitOfWork.UserRepository.GetByIdAsync(id);
                if (userPatient == null)
                {
                    throw new EntityNotFoundException("Patient", "Patient with user id " + id + " wasn't found.");
                }

                if (userPatient.Patient == null)
                {
                    throw new EntityNotFoundException("Patient", "Patient for user with id " + id + " wasn't found.");
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

                userPatient.Username = userUpdateDTO.Username!;
                userPatient.Email = userUpdateDTO.Email!;
                userPatient.Firstname = userUpdateDTO.Firstname!;
                userPatient.Lastname = userUpdateDTO.Lastname!;
                userPatient.Patient.City = userUpdateDTO.City!;
                userPatient.Patient.Address = userUpdateDTO.Address!;
                userPatient.Patient.PhoneNumber = userUpdateDTO.PhoneNumber!;

                await _unitOfWork.UserRepository.UpdateAsync(userPatient);
                await _unitOfWork.SaveAsync();
                _logger.LogInformation("{Message}", "Patient with user id " + id + " was updated successfully.");
                return _mapper.Map<UserPatientReadOnlyDTO>(userPatient);

            }
            catch (Exception ex)
            {
                _logger.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }

        }
    }
}
