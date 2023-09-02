using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TrackwiseAPI.DBContext;
using TrackwiseAPI.Models.Email;
using TrackwiseAPI.Models.Entities;
using TrackwiseAPI.Models.Interfaces;
using TrackwiseAPI.Models.Repositories;
using TrackwiseAPI.Models.ViewModels;

namespace TrackwiseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<AppUser> _claimsPrincipalFactory;
        private readonly IConfiguration _configuration;
        private readonly IAdminRepository _adminRepository;
        private readonly MailController _mailController;
        private readonly IAuditRepository _auditRepository;

        public AuditController(UserManager<AppUser> userManager,
            IUserClaimsPrincipalFactory<AppUser> claimsPrincipalFactory,
            IConfiguration configuration,
            IAdminRepository adminRepository,
            MailController mailController,
            IAuditRepository auditRepository)
        {
            _userManager = userManager;
            _claimsPrincipalFactory = claimsPrincipalFactory;
            _configuration = configuration;
            _adminRepository = adminRepository;
            _mailController = mailController;
            _auditRepository = auditRepository;
        }

        [HttpGet]
        [Route("GetAllAudit")]

        public async Task<IActionResult> GetAllAudits()
        {
            try
            {
                var results = await _auditRepository.GetAllAudtisAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

    }
}
