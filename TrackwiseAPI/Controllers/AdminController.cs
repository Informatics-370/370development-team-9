using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrackwiseAPI.Models.Entities;
using TrackwiseAPI.Models.Interfaces;
using TrackwiseAPI.Models.Repositories;
using TrackwiseAPI.Models.ViewModels;

namespace TrackwiseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;

        public AdminController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

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
        public async Task<IActionResult> GetAdminAsync(int AdminID)
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
        public async Task<IActionResult> AddAdmin(AdminVM avm)
        {
            var admin = new Admin { Name = avm.Name, Lastname = avm.Lastname, Email = avm.Email, Password = avm.Password, };

            try
            {
                _adminRepository.Add(admin);
                await _adminRepository.SaveChangesAsync();

            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }

            return Ok(admin);
        }

        //update admin
        [HttpPut]
        [Route("EditAdmin/{AdminID}")]
        public async Task<ActionResult<AdminVM>> EditAdmin(int AdminID, AdminVM avm)
        {
            try
            {
                var existingAdmin = await _adminRepository.GetAdminAsync(AdminID);
                if (existingAdmin == null) return NotFound($"The admin does not exist");

                if (existingAdmin.Name == avm.Name &&
                    existingAdmin.Lastname == avm.Lastname &&
                    existingAdmin.Email == avm.Email &&
                    existingAdmin.Password == avm.Password)
                {
                    // No changes made, return the existing driver without updating
                    return Ok(existingAdmin);
                }

                existingAdmin.Name = avm.Name;
                existingAdmin.Lastname = avm.Lastname;
                existingAdmin.Email = avm.Email;
                existingAdmin.Password = avm.Password;

                if (await _adminRepository.SaveChangesAsync())
                {
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
        public async Task<IActionResult> DeleteAdmin(int AdminID)
        {
            try
            {
                var existingAdmin = await _adminRepository.GetAdminAsync(AdminID);

                if (existingAdmin == null) return NotFound($"The admin does not exist");
                _adminRepository.Delete(existingAdmin);

                if (await _adminRepository.SaveChangesAsync()) return Ok(existingAdmin);

            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("Your request is invalid.");
        }
    }
}

