using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TrackwiseAPI.DBContext;
using TrackwiseAPI.Models.DataTransferObjects;
using TrackwiseAPI.Models.Email;
using TrackwiseAPI.Models.Entities;
using TrackwiseAPI.Models.Interfaces;
using TrackwiseAPI.Models.Repositories;

namespace TrackwiseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VATController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<AppUser> _claimsPrincipalFactory;
        private readonly IConfiguration _configuration;
        private readonly IVATRepository _VATRepository;
        public VATController(UserManager<AppUser> userManager,
            IUserClaimsPrincipalFactory<AppUser> claimsPrincipalFactory,
            IConfiguration configuration,
            IVATRepository VATRepository)
        {
            _userManager = userManager;
            _claimsPrincipalFactory = claimsPrincipalFactory;
            _configuration = configuration;
            _VATRepository = VATRepository;
        }

        [HttpGet]
        [Route("GetVAT")]
        public async Task<IActionResult> GetVAT()
        {
            try
            {
                var results = await _VATRepository.GetVATAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpPut]
        [Route("UpdateVAT/{updatedVAT}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> UpdateVAT(decimal updatedVAT)
        {
            try
            {
                var VAT = await _VATRepository.GetVATAsync();
                if (VAT == null)
                {
                    return NotFound(); // Handle VAT record not found scenario
                }

                VAT.VAT_Amount = updatedVAT;

                _VATRepository.Update(VAT);

                await _VATRepository.SaveChangesAsync();
                return Ok(VAT);
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
