using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TrackwiseAPI.DBContext;
using TrackwiseAPI.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using TrackwiseAPI.Models.Repositories;
using System.Web.Http.Results;
using TrackwiseAPI.Controllers;


namespace TrackwiseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobRuleController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<AppUser> _claimsPrincipalFactory;
        private readonly IConfiguration _configuration;
        private readonly IVATRepository _VATRepository;
        private readonly IJobRuleRepository _jobRuleRepository;
        public JobRuleController(UserManager<AppUser> userManager,
            IUserClaimsPrincipalFactory<AppUser> claimsPrincipalFactory,
            IConfiguration configuration,
            IVATRepository VATRepository,
            IJobRuleRepository jobRuleRepository)
        {
            _userManager = userManager;
            _claimsPrincipalFactory = claimsPrincipalFactory;
            _configuration = configuration;
            _VATRepository = VATRepository;
            _jobRuleRepository = jobRuleRepository;
        }

        [HttpGet]
        [Route("Getbreak")]
        public async Task<double> GetBreak()
        {
            try
            {
                var results = await _jobRuleRepository.GetRuleAsync();
                double breakValue = results.Break; 
                return breakValue;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public class BreakVM
        {
            public double updatebreak { get; set; }
        }

        [HttpPut]
        [Route("UpdateBreak/{updatedbreak}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<BreakVM>> UpdateBreak(BreakVM bvm)
        {
            try
            {
                var rule = await _jobRuleRepository.GetRuleAsync();
                if (rule == null)
                {
                    return NotFound(); 
                }

                rule.Break = bvm.updatebreak;
                await _jobRuleRepository.SaveChangesAsync();
                return Ok(rule);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine(ex);

                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }


    }
}
