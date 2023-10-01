using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class DriverController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<AppUser> _claimsPrincipalFactory;
        private readonly IConfiguration _configuration;
        private readonly IDriverRepository _driverRepository;
        private readonly MailController _mailController;
        private readonly IAuditRepository _auditRepository;
        public DriverController(UserManager<AppUser> userManager,
            IUserClaimsPrincipalFactory<AppUser> claimsPrincipalFactory,
            IConfiguration configuration,
            IDriverRepository driverRepository,
            MailController mailController,
            IAuditRepository auditRepository)
        {
            _userManager = userManager;
            _claimsPrincipalFactory = claimsPrincipalFactory;
            _configuration = configuration;
            _driverRepository = driverRepository;
            _mailController = mailController;
            _auditRepository = auditRepository;
        }

        [HttpGet]
        [Route("GetAllDrivers")]
        public async Task<IActionResult> GetAllDrivers()
        {
            try
            {
                var results = await _driverRepository.GetAllDriversAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        //Get a specific Driver
        [HttpGet]
        [Route("GetDriver/{Driver_ID}")]
        public async Task<IActionResult> GetDriverAsync(string Driver_ID)
        {
            try
            {
                var result = await _driverRepository.GetDriverAsync(Driver_ID);

                if (result == null) return NotFound("Driver does not exist");

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support");
            }
        }

        //Add a Driver
        [HttpPost]
        [Route("AddDriver")]
        public async Task<IActionResult> AddDriver(DriverVM dvm)
        {
            var driverId = Guid.NewGuid().ToString();
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var auditId = Guid.NewGuid().ToString();
            var audit = new Audit { Audit_ID = auditId, Action = "Add Driver", CreatedDate = DateTime.Now, User = userEmail };

            var driver = new Driver { Driver_ID = driverId, Name = dvm.Name, Lastname = dvm.Lastname, Email = dvm.Email ,PhoneNumber = dvm.PhoneNumber, Driver_Status_ID = "1" };
            var newdrivermail = new NewDriverMail { Email = driver.Email, Name = driver.Name, PhoneNumber = driver.PhoneNumber, Driver_Status_ID = "1", Password = dvm.Password };
            var existingadmin = await _userManager.FindByNameAsync(dvm.Email);
            if (existingadmin != null) return BadRequest("User already exists");

            try
            {
                _driverRepository.Add(driver);
                await _driverRepository.SaveChangesAsync();
                _auditRepository.Add(audit);
                await _auditRepository.SaveChangesAsync();

                var user = new AppUser
                {
                    Id = driverId,
                    UserName = dvm.Email,
                    Email = dvm.Email,
                    EmailConfirmed = true
            };

                var result = await _userManager.CreateAsync(user, dvm.Password);
                var mail = await _mailController.SendDriverEmail(newdrivermail);

                await _userManager.AddToRoleAsync(user, "Driver");

                if (result.Errors.Count() > 0)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error. Please contact support.");

            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }

            return Ok(driver);
        }

        //update Driver
        [HttpPut]
        [Route("EditDriver/{Driver_ID}")]
        public async Task<ActionResult<DriverVM>> EditDriver(string Driver_ID, DriverVM dvm)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var auditId = Guid.NewGuid().ToString();
            var audit = new Audit { Audit_ID = auditId, Action = "Update Driver", CreatedDate = DateTime.Now, User = userEmail };
            try
            {
                var existingDriver = await _driverRepository.GetDriverAsync(Driver_ID);
                if (existingDriver == null)
                    return NotFound($"The driver does not exist");

                var existingUser = await _userManager.FindByIdAsync(Driver_ID);
                if (existingUser == null)
                    return NotFound($"The corresponding user does not exist");

                // Check if any changes are made to the driver details
                if (existingDriver.Name == dvm.Name &&
                    existingDriver.Lastname == dvm.Lastname &&
                    existingDriver.Email == dvm.Email &&
                    existingDriver.PhoneNumber == dvm.PhoneNumber)
                {
                    // No changes made, return the existing driver without updating
                    return Ok(existingDriver);
                }

                existingDriver.Name = dvm.Name;
                existingDriver.Lastname = dvm.Lastname;
                existingDriver.Email = dvm.Email;
                existingDriver.PhoneNumber = dvm.PhoneNumber;

                existingUser.UserName = dvm.Email;
                existingUser.Email = dvm.Email;
                await _userManager.RemovePasswordAsync(existingUser);
                await _userManager.AddPasswordAsync(existingUser, dvm.Password);
                existingUser.SecurityStamp = Guid.NewGuid().ToString();

                var driverUpdateResult = await _driverRepository.SaveChangesAsync();
                var userUpdateResult = await _userManager.UpdateAsync(existingUser);

                if (driverUpdateResult && userUpdateResult.Succeeded)
                {
                    _auditRepository.Add(audit);
                    await _auditRepository.SaveChangesAsync();
                    return Ok(existingDriver);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("Your request is invalid.");
        }

        //Remove Driver
        [HttpDelete]
        [Route("DeleteDriver/{Driver_ID}")]
        public async Task<IActionResult> DeleteDriver(string Driver_ID)
        {
            try
            {
                var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var auditId = Guid.NewGuid().ToString();
                var audit = new Audit { Audit_ID = auditId, Action = "Delete Driver", CreatedDate = DateTime.Now, User = userEmail };
                var existingDriver = await _driverRepository.GetDriverAsync(Driver_ID);

                if (existingDriver == null) 
                    return NotFound($"The driver does not exist");

                var user = await _userManager.FindByEmailAsync(existingDriver.Email);
                if (user != null)
                {
                    var result = await _userManager.DeleteAsync(user);
                    if (!result.Succeeded)
                    {
                        return StatusCode(500, "Failed to delete the associated user.");
                    }
                }

                _driverRepository.Delete(existingDriver);

                if (await _driverRepository.SaveChangesAsync())
                {
                    _auditRepository.Add(audit);
                    await _auditRepository.SaveChangesAsync();
                    return Ok(existingDriver);
                }
                   

            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("Your request is invalid.");
        }
    }

}
