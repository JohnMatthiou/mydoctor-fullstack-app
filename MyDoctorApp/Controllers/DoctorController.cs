using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyDoctorApp.Core.Filters;
using MyDoctorApp.DTO;
using MyDoctorApp.Exceptions;
using MyDoctorApp.Services;

namespace MyDoctorApp.Controllers
{
    [Route("api/doctors")]
    public class DoctorController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public DoctorController(IApplicationService applicationService, IConfiguration configutarion, IMapper mapper) : base(applicationService)
        {
            _configuration = configutarion;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a doctor's details based on their associated user ID.
        /// </summary>
        /// <param name="id">The user ID of the doctor to retrieve.</param>
        /// <returns>
        /// The details of the doctor as a <see cref="UserDoctorReadOnlyDTO"/> object.
        /// </returns>
        /// <response code="200">
        /// Returns the details of the doctor associated with the specified user ID.
        /// </response>
        /// <response code="401">
        /// <para><b>Unauthorized</b></para>
        /// <para>The client is not authorized to access this endpoint. A valid JWT token is required.</para>
        /// </response>
        /// <response code="403">
        /// <para><b>Forbidden</b></para>
        /// <para>The logged-in user does not have the required role to access this resource.</para>
        /// </response>
        /// <response code="404">
        /// No doctor was found for the specified user ID.
        /// </response>
        /// <response code="500">
        /// An internal server error occurred while processing the request.
        /// </response>
        [HttpGet("user/{id}")]
        [Authorize(Roles = "Doctor, Patient")]
        public async Task<ActionResult<UserDoctorReadOnlyDTO>> GetByUserIdAsync(int id)
        {
            UserDoctorReadOnlyDTO returnedDoctorDTO = await _applicationService.DoctorService.GetByUserIdAsync(id);
            return Ok(returnedDoctorDTO);
        }

        /// <summary>
        /// Updates the details of the logged-in doctor. The user ID in the request must match the logged-in user's ID.
        /// </summary>
        /// <param name="id">The user ID of the doctor to update.</param>
        /// <param name="userUpdateDTO">
        /// The updated details of the doctor, provided as a <see cref="UserUpdateDTO"/> object.
        /// </param>
        /// <returns>
        /// The updated details of the doctor as a <see cref="UserDoctorReadOnlyDTO"/> object.
        /// </returns>
        /// <response code="200">
        /// Returns the updated details of the doctor.
        /// </response>
        /// <response code="400">
        /// The request contains invalid data, such as validation errors or a mismatch between the route ID and the body ID or
        /// a conflict occurred because a user or doctor with the specified username, email, or phone number already exists.
        /// </response>
        /// <response code="401">
        /// <para><b>Unauthorized</b></para>
        /// <para>The client is not authorized to access this endpoint. A valid JWT token is required.</para>
        /// </response>
        /// <response code="403">
        /// <para><b>Forbidden</b></para>
        /// <para>The logged-in user is not authorized to update the specified doctor's details because the user ID does not match the logged-in user's ID or
        /// the logged-in user does not have the required role ('Doctor') to access this resource.</para>
        /// </response>
        /// <response code="404">
        /// The doctor associated with the specified user ID could not be found.
        /// </response>
        /// <response code="500">
        /// An internal server error occurred while processing the request.
        /// </response>
        [HttpPatch("user/{id}")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<UserDoctorReadOnlyDTO>> UpdateUserDoctorAsync(int id, UserUpdateDTO userUpdateDTO)
        {
            var userId = AppUser!.Id;
            if (id != userId)
            {
                throw new EntityForbiddenException("Access", "User id in the request does not match logged-in user's id.");
            }

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
            UserDoctorReadOnlyDTO returnedDoctorDTO = await _applicationService.DoctorService.UpdateUserAsync(id, userUpdateDTO);
            return Ok(returnedDoctorDTO);
        }

        /// <summary>
        /// Removes a patient from the logged-in doctor's list of associated patients.
        /// </summary>
        /// <param name="userIdDoctor">The user ID of the doctor (must match the logged-in user's ID) performing the removal.</param>
        /// <param name="userIdPatient">The user ID of the patient to be removed from the doctor's list of patients.</param>
        /// <returns>
        /// A confirmation message indicating that the patient was successfully removed from the doctor's list of patients.
        /// </returns>
        /// <response code="200">
        /// The patient was successfully removed from the doctor's list of patients.
        /// </response>
        /// <response code="401">
        /// <para><b>Unauthorized</b></para>
        /// <para>The client is not authorized to access this endpoint. A valid JWT token is required.</para>
        /// </response>
        /// <response code="403">
        /// <para><b>Forbidden</b></para>
        /// <para>The user ID in the request does not match the logged-in user's ID, preventing access to this operation or
        /// client does not have the required role ('Doctor') to access this resource.</para>
        /// </response>
        /// <response code="404">
        /// The specified doctor, patient, or association between them was not found.
        /// </response>
        /// <response code="500">
        /// An internal server error occurred while processing the request.
        /// </response>
        [HttpDelete("{userIdDoctor}/patients/{userIdPatient}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> RemovePatientFromDoctorAsync(int userIdDoctor, int userIdPatient)
        {
            var userId = AppUser!.Id;
            if (userIdDoctor != userId)
            {
                throw new EntityForbiddenException("Access", "User id in the request does not match logged-in user's id.");
            }

            await _applicationService.DoctorService.RemovePatientFromDoctorAsync(userIdDoctor, userIdPatient);
            return Ok(new { message = "Patient removed successfully." });
        }

        /// <summary>
        /// Retrieves a paginated list of patients associated with the logged-in doctor.
        /// </summary>
        /// <param name="userIdDoctor">The user ID of the doctor (must match the logged-in user's ID).</param>
        /// <param name="pageNumber">The page number to retrieve. Must be greater than zero. Defaults to 1.</param>
        /// <param name="pageSize">The number of records per page. Must be greater than zero. Defaults to 10.</param>
        /// <returns>
        /// A paginated result containing the list of patients associated with the doctor.
        /// </returns>
        /// <response code="200">
        /// Returns the paginated list of patients associated with the doctor.
        /// </response>
        /// <response code="400">
        /// The provided pagination parameters (page number or page size) are invalid.
        /// </response>
        /// <response code="401">
        /// <para><b>Unauthorized</b></para>
        /// <para>The client is not authorized to access this endpoint. A valid JWT token is required.</para>
        /// </response>
        /// <response code="403">
        /// <para><b>Forbidden</b></para>
        /// <para>The user ID in the request does not match the logged-in user's ID, preventing access to this operation or
        /// client does not have the required role ('Doctor') to access this resource.</para>
        /// </response>
        /// <response code="404">
        /// The specified doctor was not found, or no patients were found for the given doctor.
        /// </response>
        /// <response code="500">
        /// An internal server error occurred while processing the request.
        /// </response>
        [HttpGet("{userIdDoctor}/patients")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<PaginatedResultDTO<UserPatientReadOnlyDTO>>> GetDoctorPatientsPaginatedAsync(int userIdDoctor,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var userId = AppUser!.Id;
            if (userIdDoctor != userId)
            {
                throw new EntityForbiddenException("Access", "User id in the request does not match logged-in user's id.");
            }

            if (pageNumber <= 0)
            {
                throw new InvalidArgumentException("Page number", "Page number must be greater than zero.");
            }

            if (pageSize <= 0)
            {
                throw new InvalidArgumentException("Page size", "Page size must be greater than zero.");
            }

            PaginatedResultDTO<UserPatientReadOnlyDTO> paginatedResultDTO = await _applicationService.DoctorService.GetDoctorPatientsPaginatedAsync(userIdDoctor, pageNumber, pageSize);
            return Ok(paginatedResultDTO);
        }

        /// <summary>
        /// Retrieves a paginated list of doctors based on specified filter criteria.
        /// </summary>
        /// <param name="filters">The filtering criteria used to search for doctors.</param>
        /// <param name="pageNumber">The page number to retrieve. Must be greater than zero. Defaults to 1.</param>
        /// <param name="pageSize">The number of records per page. Must be greater than zero. Defaults to 10.</param>
        /// <returns>
        /// A paginated result containing a list of doctors that match the filter criteria.
        /// </returns>
        /// <response code="200">
        /// A paginated list of doctors successfully retrieved based on the filters.
        /// </response>
        /// <response code="400">
        /// The request contains invalid data, such as non-positive page numbers or page sizes.
        /// </response>
        /// <response code="401">
        /// <para><b>Unauthorized</b></para>
        /// <para>The client is not authorized to access this endpoint. A valid JWT token is required.</para>
        /// </response>
        /// <response code="403">
        /// <para><b>Forbidden</b></para>
        /// <para>The client does not have the required role ('Patient') to access this resource.</para>
        /// </response>
        /// <response code="404">
        /// No doctors were found that match the specified filtering criteria.
        /// </response>
        /// <response code="500">
        /// An internal server error occurred while processing the request.
        /// </response>
        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult<PaginatedResultDTO<UserDoctorReadOnlyDTO>>> GetDoctorsFilteredPaginatedAsync(DoctorFilters filters, [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            if (pageNumber <= 0)
            {
                throw new InvalidArgumentException("Page number", "Page number must be greater than zero.");
            }

            if (pageSize <= 0)
            {
                throw new InvalidArgumentException("Page size", "Page size must be greater than zero.");
            }

            PaginatedResultDTO<UserDoctorReadOnlyDTO> paginatedResultDTO = await _applicationService.DoctorService.GetDoctorsFilteredPaginatedAsync(filters, pageNumber, pageSize);
            return Ok(paginatedResultDTO);
         }

    }
}
