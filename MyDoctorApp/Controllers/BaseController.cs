using Microsoft.AspNetCore.Mvc;
using MyDoctorApp.Models;
using MyDoctorApp.Services;
using System.Security.Claims;

namespace MyDoctorApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public readonly IApplicationService _applicationService;

        protected BaseController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        private ApplicationUser? _appUser;

        protected ApplicationUser? AppUser
        {
            get
            {
                if (User != null && User.Claims != null && User.Claims.Any())
                {

                    var claimsTypes = User.Claims.Select(x => x.Type);
                    if (!claimsTypes.Contains(ClaimTypes.NameIdentifier))
                    {
                        return null;
                    }

                    
                    var userClaimsId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    _ = int.TryParse(userClaimsId, out int id);

                    _appUser = new ApplicationUser
                    {
                        Id = id
                    };

                    _appUser.Email = User.FindFirst(ClaimTypes.Email)?.Value;
                    return _appUser;
                }
                return null;
            }
        }
    }
}
