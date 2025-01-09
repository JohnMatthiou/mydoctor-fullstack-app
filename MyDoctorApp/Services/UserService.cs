using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using MyDoctorApp.Core.Enums;
using MyDoctorApp.Data;
using MyDoctorApp.DTO;
using MyDoctorApp.Repositories;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyDoctorApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = new LoggerFactory().AddSerilog().CreateLogger<UserService>();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            User? user = null;

            try
            {
                user = await _unitOfWork.UserRepository.GetAsync(id);
            }
            catch (Exception e)
            {
                _logger.LogError("{Message}{Excpetion}", e.Message, e.StackTrace);
            }
            return user;
        }

        public string CreateUserToken(int userId, string email, string firstname, string lastname, UserRole? userRole,
            string appSecurityKey)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSecurityKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claimsInfo = new List<Claim>
            {
                new Claim(ClaimTypes.Name, $"{firstname} {lastname}"),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, userRole.ToString()!)
            };

            var jwtSecurityToken = new JwtSecurityToken(null, null, claimsInfo, DateTime.UtcNow,
                DateTime.UtcNow.AddHours(3), signingCredentials);

            var userToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return userToken;
        }

        public async Task<User?> VerifyAndGetUserAsync(UserLoginDTO credentials)
        {
            User? user;

            try
            {
                user = await _unitOfWork.UserRepository.GetUserAsync(credentials.Username!, credentials.Password!);
                if (user != null)
                {
                    _logger.LogInformation("User: Id: {Id}, Username: {Username}, Role: {Role} found and returned.",
                        user.Id, user.Username, user.UserRole);
                }
                else
                {
                    _logger.LogWarning("No user found for provided credentials. Username: {Username}", credentials.Username);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }
            return user;
        }
    }
    
}
