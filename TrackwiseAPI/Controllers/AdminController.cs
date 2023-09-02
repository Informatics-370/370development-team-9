using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    public class AdminController : ControllerBase
    {


        private readonly UserManager<AppUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<AppUser> _claimsPrincipalFactory;
        private readonly IConfiguration _configuration;
        private readonly IAdminRepository _adminRepository;
        private readonly MailController _mailController;
        private readonly IAuditRepository _auditRepository;
        public AdminController(UserManager<AppUser> userManager,
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

        /*        public AdminController(IAdminRepository adminRepository)
                {
                    _adminRepository = adminRepository;
                }*/

        //Get all admins
        [HttpGet]
        [Route("GetAllAdmin")]
        
        public async Task<IActionResult> GetAllAdmins()
        {
            try
            {
                var results = await _adminRepository.GetAllAdminsAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        //Get a specific admin
        [HttpGet]
        [Route("GetAdmin/{AdminID}")]

        public async Task<IActionResult> GetAdminAsync(string AdminID)
        {
            try
            {
                var result = await _adminRepository.GetAdminAsync(AdminID);

                if (result == null) return NotFound("Admin does not exist");

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support");
            }
        }

        //Add a admin
        [HttpPost]
        [Route("AddAdmin")]
        public async Task<IActionResult> AddNewAdmin(AdminVM avm)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var auditId = Guid.NewGuid().ToString();
            var audit = new Audit { Audit_ID = auditId, Action = "Add Admin", CreatedDate = DateTime.Now, User = userEmail };
            var adminId = Guid.NewGuid().ToString();
            var admin = new Admin { Admin_ID = adminId, Name = avm.Name, Lastname = avm.Lastname, Email = avm.Email };
            var existingadmin = await _userManager.FindByNameAsync(avm.Email);
            if (existingadmin != null) return BadRequest("User already exists");

            var newadminmail = new NewAdminMail { Email = admin.Email, Name = admin.Name, Password = avm.Password };
            try
            {
                _adminRepository.Add(admin);
                await _adminRepository.SaveChangesAsync();
                _auditRepository.Add(audit);
                await _auditRepository.SaveChangesAsync();
                var user = new AppUser
                {
                    Id = adminId,
                    UserName = avm.Email,
                    Email = avm.Email
                };

                var result = await _userManager.CreateAsync(user, avm.Password);
                var mail = await _mailController.SendAdminEmail(newadminmail);

                await _userManager.AddToRoleAsync(user, "Admin");

                if (result.Errors.Count() > 0)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error. Please contact support.");

            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }

            return Ok(admin);
        }


        [HttpPut]
        [Route("EditAdmin/{AdminID}")]
        public async Task<ActionResult<AdminVM>> EditAdmin(string AdminID, AdminVM avm)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var auditId = Guid.NewGuid().ToString();
            var audit = new Audit { Audit_ID = auditId, Action = "Update Admin", CreatedDate = DateTime.Now, User = userEmail };
            try
            {
                var existingAdmin = await _adminRepository.GetAdminAsync(AdminID);
                if (existingAdmin == null)
                    return NotFound($"The admin does not exist");

                var existingUser = await _userManager.FindByIdAsync(AdminID);
                if (existingUser == null)
                    return NotFound($"The corresponding user does not exist");

                if (existingAdmin.Name == avm.Name &&
                    existingAdmin.Lastname == avm.Lastname &&
                    existingAdmin.Email == avm.Email)
                {
                    // No changes made, return the existing admin without updating
                    return Ok(existingAdmin);
                }

                existingAdmin.Name = avm.Name;
                existingAdmin.Lastname = avm.Lastname;
                existingAdmin.Email = avm.Email;

                existingUser.UserName = avm.Email;
                existingUser.Email = avm.Email;

                await _userManager.RemovePasswordAsync(existingUser);
                await _userManager.AddPasswordAsync(existingUser, avm.Password);
                existingUser.SecurityStamp = Guid.NewGuid().ToString();

                var adminUpdateResult = await _adminRepository.SaveChangesAsync();
                var userUpdateResult = await _userManager.UpdateAsync(existingUser);

                if (adminUpdateResult && userUpdateResult.Succeeded)
                {
                    _auditRepository.Add(audit);
                    await _auditRepository.SaveChangesAsync();
                    return Ok(existingAdmin);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }

            return BadRequest("Your request is invalid.");
        }



        //Remove admin
        [HttpDelete]
        [Route("DeleteAdmin/{AdminID}")]
        public async Task<IActionResult> DeleteAdmin(string AdminID)
        {
            try
            {
                var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var auditId = Guid.NewGuid().ToString();
                var audit = new Audit { Audit_ID = auditId, Action = "Delete Admin", CreatedDate = DateTime.Now, User = userEmail };
                var existingAdmin = await _adminRepository.GetAdminAsync(AdminID);

                if (existingAdmin == null) 
                    return NotFound($"The admin does not exist");

                var user = await _userManager.FindByEmailAsync(existingAdmin.Email);
                if (user != null)
                {
                    var result = await _userManager.DeleteAsync(user);
                    if (!result.Succeeded)
                    {
                        return StatusCode(500, "Failed to delete the associated user.");
                    }
                }

                _adminRepository.Delete(existingAdmin);

                if (await _adminRepository.SaveChangesAsync())
                {
                    _auditRepository.Add(audit);
                    await _auditRepository.SaveChangesAsync();
                    return Ok(existingAdmin);
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

