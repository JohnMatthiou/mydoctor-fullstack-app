using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyDoctorApp.DTO;
using MyDoctorApp.Exceptions;
using MyDoctorApp.Services;

namespace MyDoctorApp.Controllers
{
    [Route("api/patients")]
    public class PatientController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public PatientController(IApplicationService applicationService, IConfiguration configutarion, IMapper mapper) : base(applicationService)
        {
            _configuration = configutarion;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a patient's details based on their associated user ID.
        /// </summary>
        /// <param name="id">The user ID associated with the patient to retrieve.</param>
        /// <returns>
        /// A <see cref="UserPatientReadOnlyDTO"/> object containing the patient's details.
        /// </returns>
        /// <response code="200">
        /// The patient's details were successfully retrieved and are returned in the response.
        /// </response>
        /// <response code="401">
        /// <para><b>Unauthorized</b></para>
        /// <para>The client is not authorized to access this endpoint. A valid JWT token with the required roles ('Patient' or 'Doctor') is needed.</para>
        /// </response>
        /// <response code="403">
        /// <para><b>Forbidden</b></para>
        /// <para>The client does not have the required role ('Patient' or 'Doctor') to access this resource.</para>
        /// </response>
        /// <response code="404">No patient was found with the provided user ID.</response>
        /// <response code="500">An internal server error occurred while processing the request.</response>
        [HttpGet("user/{id}")]
        [Authorize(Roles = "Patient, Doctor")]
        public async Task<ActionResult<UserPatientReadOnlyDTO>> GetByUserIdAsync(int id)
        {
            UserPatientReadOnlyDTO returnedPatientDTO = await _applicationService.PatientService.GetByUserIdAsync(id);
            return Ok(returnedPatientDTO);
        }

        /// <summary>
        /// Updates the details of the logged-in patient. The user ID in the request must match the logged-in user's ID.
        /// </summary>
        /// <param name="id">The user ID associated with the patient whose information needs to be updated. This must match the logged-in user's ID.</param>
        /// <param name="userUpdateDTO">The updated user details including username, email, and patient information such as city, address, and phone number.</param>
        /// <returns>
        /// A <see cref="UserPatientReadOnlyDTO"/> object containing the updated patient details.
        /// </returns>
        /// <response code="200">
        /// The patient's details were successfully updated, and the updated information is returned in the response.
        /// </response>
        /// <response code="400">
        /// The provided model data is invalid or incomplete, and cannot be processed. A conflict occurred, such as an existing username or email that conflicts with another user in the system.
        /// </response>
        /// <response code="401">
        /// <para><b>Unauthorized</b></para>
        /// <para>The client is not authorized to access this endpoint. A valid JWT token is required.</para>
        /// </response>
        /// <response code="403">
        /// <para><b>Forbidden</b></para>
        /// <para>The user ID in the request does not match the logged-in user's ID, preventing access to the update operation or
        /// client does not have the required role ('Patient') to access this resource.</para>
        /// </response>
        /// <response code="404">
        /// No patient was found for the provided user ID, or the patient is not associated with the user account.
        /// </response>
        /// <response code="500">
        /// An internal server error occurred while processing the update request.
        /// </response>
        [HttpPatch("user/{id}")]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult<UserPatientReadOnlyDTO>> UpdateUserPatientAsync(int id, UserUpdateDTO userUpdateDTO)
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
            UserPatientReadOnlyDTO returnedPatientDTO = await _applicationService.PatientService.UpdateUserAsync(id, userUpdateDTO);
            return Ok(returnedPatientDTO);
        }

        /// <summary>
        /// Adds a doctor to the logged-in patient's list of doctors.
        /// </summary>
        /// <param name="userIdPatient">The user ID of the patient (must match the logged-in user's ID).</param>
        /// <param name="userIdDoctor">The user ID of the doctor to be added to the patient's list of doctors.</param>
        /// <returns>
        /// A confirmation message indicating the doctor was successfully added to the patient's list of doctors.
        /// </returns>
        /// <response code="200">
        /// The doctor was successfully added to the patient's list of doctors.
        /// </response>
        /// <response code="400">
        /// The specified doctor is already associated with the patient's list of doctors.
        /// </response>
        /// <response code="401">
        /// <para><b>Unauthorized</b></para>
        /// <para>The client is not authorized to access this endpoint. A valid JWT token is required.</para>
        /// </response>
        /// <response code="403">
        /// <para><b>Forbidden</b></para>
        /// <para>The user ID in the request does not match the logged-in user's ID, preventing access to this operation or
        /// client does not have the required role ('Patient') to access this resource.</para>
        /// </response>
        /// <response code="404">
        /// The specified patient or doctor was not found in the system.
        /// </response>
        /// <response code="500">
        /// An internal server error occurred while processing the request.
        /// </response>
        [HttpPost("{userIdPatient}/doctors/{userIdDoctor}")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> AddDoctorToPatientAsync(int userIdPatient, int userIdDoctor)
        {
            var userId = AppUser!.Id;
            if (userIdPatient != userId)
            {
                throw new EntityForbiddenException("Access", "User id in the request does not match logged-in user's id.");
            }

            await _applicationService.PatientService.AddDoctorToPatientAsync(userIdPatient, userIdDoctor);
            return Ok(new { message = "Doctor added successfully." });
        }

        /// <summary>
        /// Removes a doctor from the logged-in patient's list of associated doctors.
        /// </summary>
        /// <param name="userIdPatient">The user ID of the patient (must match the logged-in user's ID) performing the removal.</param>
        /// <param name="userIdDoctor">The user ID of the doctor to be removed from the patient's list of doctors.</param>
        /// <returns>
        /// A confirmation message indicating the doctor was successfully removed from the patient's list of doctors.
        /// </returns>
        /// <response code="200">
        /// The doctor was successfully removed from the patient's list of doctors.
        /// </response>
        /// <response code="401">
        /// <para><b>Unauthorized</b></para>
        /// <para>The client is not authorized to access this endpoint. A valid JWT token is required.</para>
        /// </response>
        /// <response code="403">
        /// <para><b>Forbidden</b></para>
        /// <para>The user ID in the request does not match the logged-in user's ID, preventing access to this operation or
        /// client does not have the required role ('Patient') to access this resource.</para>
        /// </response>
        /// <response code="404">
        /// The specified patient, doctor, or association between them was not found.
        /// </response>
        /// <response code="500">
        /// An internal server error occurred while processing the request.
        /// </response>
        [HttpDelete("{userIdPatient}/doctors/{userIdDoctor}")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> RemoveDoctorFromPatientAsync(int userIdPatient, int userIdDoctor)
        {
            var userId = AppUser!.Id;
            if (userIdPatient != userId)
            {
                throw new EntityForbiddenException("Access", "User id in the request does not match logged-in user's id.");
            }

            await _applicationService.PatientService.RemoveDoctorFromPatientAsync(userIdPatient, userIdDoctor);
            return Ok(new { message = "Doctor removed successfully." });
        }

        /// <summary>
        /// Retrieves a paginated list of doctors associated with the logged-in patient.
        /// </summary>
        /// <param name="userIdPatient">The user ID of the patient (must match the logged-in user's ID).</param>
        /// <param name="pageNumber">The page number to retrieve. Must be greater than zero. Defaults to 1.</param>
        /// <param name="pageSize">The number of records per page. Must be greater than zero. Defaults to 10.</param>
        /// <returns>
        /// A paginated result containing a list of doctors associated with the patient.
        /// </returns>
        /// <response code="200">
        /// Returns the paginated list of doctors associated with the patient.
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
        /// client does not have the required role ('Patient') to access this resource.</para>
        /// </response>
        /// <response code="404">
        /// The specified patient was not found, or no doctors were found for the given patient.
        /// </response>
        /// <response code="500">
        /// An internal server error occurred while processing the request.
        /// </response>
        [HttpGet("{userIdPatient}/doctors")]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult<PaginatedResultDTO<UserDoctorReadOnlyDTO>>> GetPatientDoctorsPaginatedAsync(int userIdPatient,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var userId = AppUser!.Id;
            if (userIdPatient != userId)
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

            PaginatedResultDTO<UserDoctorReadOnlyDTO> paginatedResultDTO = await _applicationService.PatientService.GetPatientDoctorsPaginatedAsync(userIdPatient, pageNumber, pageSize);
            return Ok(paginatedResultDTO);
        }

    }
}
