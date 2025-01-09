using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyDoctorApp.DTO;
using MyDoctorApp.Exceptions;
using MyDoctorApp.Services;

namespace MyDoctorApp.Controllers
{
    [Route("api/users")]
    public class UserController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public UserController(IApplicationService applicationService, IConfiguration configuration,
            IMapper mapper) : base(applicationService)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        /// <summary>
        /// Registers a new user as a patient.
        /// </summary>
        /// <param name="patientSignupDTO">
        /// The data required to register the user as a patient, including personal details and credentials.
        /// </param>
        /// <returns>
        /// A newly created <see cref="UserReadOnlyDTO"/> object representing the registered user with their basic information.
        /// </returns>
        /// <response code="201">The user was successfully registered, and the new resource has been created.</response>
        /// <response code="400">
        /// The request contains validation errors, such as missing or invalid data in the <paramref name="patientSignupDTO"/>,
        /// or a conflict occurred during registration, such as a username, email, or AMKA that is already in use.
        /// </response>
        /// <response code="500">An internal server error occurred while processing the request.</response>
        [HttpPost("patients")]
        public async Task<ActionResult<UserReadOnlyDTO>> SignupUserPatientAsync(PatientSignupDTO? patientSignupDTO)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(e => e.Value!.Errors.Any())
                    .Select(e => new
                    {
                        Field = e.Key,
                        Errors = e.Value!.Errors.Select(error => error.ErrorMessage).ToArray()
                    });

                throw new InvalidRegistrationException("ErrorsInRegistation: " + errors);
            }
            if (_applicationService == null)
            {
                throw new ServerException("ApplicationServiceNull", "Application Service is null");
            }
            UserReadOnlyDTO returnedUserDTO = await _applicationService.PatientService.SignUpUserAsync(patientSignupDTO!);

            return CreatedAtAction(nameof(GetUserById), new { id = returnedUserDTO.Id }, returnedUserDTO);
        }

        /// <summary>
        /// Registers a new user as a doctor.
        /// </summary>
        /// <param name="doctorSignupDTO">
        /// The data required to register the user as a doctor, including personal details, credentials, and doctor-specific information.
        /// </param>
        /// <returns>
        /// A newly created <see cref="UserReadOnlyDTO"/> object representing the registered user with their basic information.
        /// </returns>
        /// <response code="201">The user was successfully registered, and the new resource has been created.</response>
        /// <response code="400">
        /// The request contains validation errors, such as missing or invalid data in the <paramref name="doctorSignupDTO"/>,
        /// or a conflict occurred during registration, such as a username, email, AFM, or phone number that is already in use.
        /// </response>
        /// <response code="500">An internal server error occurred while processing the request.</response>
        [HttpPost("doctors")]
        public async Task<ActionResult<UserReadOnlyDTO>> SignupUserDoctorAsync(DoctorSignupDTO? doctorSignupDTO)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(e => e.Value!.Errors.Any())
                    .Select(e => new
                    {
                        Field = e.Key,
                        Errors = e.Value!.Errors.Select(error => error.ErrorMessage).ToArray()
                    });

                throw new InvalidRegistrationException("ErrorsInRegistation: " + errors);
            }
            if (_applicationService == null)
            {
                throw new ServerException("ApplicationServiceNull", "Application Service is null");
            }
            UserReadOnlyDTO returnedUserDTO = await _applicationService.DoctorService.SignUpUserAsync(doctorSignupDTO!);

            return CreatedAtAction(nameof(GetUserById), new { id = returnedUserDTO.Id }, returnedUserDTO);
        }

        /// <summary>
        /// Retrieves a user's details by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to retrieve.</param>
        /// <returns>
        /// A <see cref="UserReadOnlyDTO"/> object containing the user's basic information.
        /// </returns>
        /// <response code="200">The user was successfully retrieved.</response>
        /// <response code="401">
        /// <para><b>Unauthorized</b></para>
        /// <para>The client is not authorized to access this endpoint. A valid JWT token is required.</para>
        /// </response>
        /// <response code="403">
        /// <para><b>Forbidden</b></para>
        /// <para>The client does not have sufficient permissions to access this resource.</para>
        /// </response>
        /// <response code="404">The specified user was not found.</response>
        /// <response code="500">An internal server error occurred while processing the request.</response>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UserReadOnlyDTO>> GetUserById(int id)
        {
            var user = await _applicationService.UserService.GetUserByIdAsync(id) ?? throw new EntityNotFoundException("User", "User: " + id + " NotFound");
            var returnedDto = _mapper.Map<UserReadOnlyDTO>(user);
            return Ok(returnedDto);
        }

        /// <summary>
        /// Authenticates a user and generates a JWT token.
        /// </summary>
        /// <param name="credentials">
        /// The user's login credentials, including username and password.
        /// </param>
        /// <returns>
        /// A <see cref="JwtTokenDTO"/> object containing the generated JWT token if authentication is successful.
        /// </returns>
        /// <response code="200">
        /// <para><b>Success</b></para>
        /// <para>The user was successfully authenticated, and a JWT token is returned in the response.</para>
        /// </response>
        /// <response code="400">The request contains validation errors, such as missing or invalid data in in the <paramref name="credentials"/></response>
        /// <response code="401">
        /// <para><b>Unauthorized</b></para>
        /// <para>The user's credentials are incorrect, or the user is not authorized to access the system.</para>
        /// </response>
        /// <response code="500">An internal server error occurred while processing the request.</response>
        [HttpPost("login")]
        public async Task<ActionResult<JwtTokenDTO>> LoginUserAsync(UserLoginDTO credentials)
        {
            var user = await _applicationService.UserService.VerifyAndGetUserAsync(credentials);
            if (user == null)
            {
                throw new EntityNotAuthorizedException("User", "BadCredentials");
            }

            var userToken = _applicationService.UserService.CreateUserToken(user.Id, user.Email!, user.Firstname,
               user.Lastname, user.UserRole, _configuration["Authentication:SecretKey"]!);

            JwtTokenDTO token = new()
            {
                Token = userToken
            };

            return Ok(token);
        }

    }
}
