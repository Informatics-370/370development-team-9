using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Runtime.Intrinsics.X86;
using TrackwiseAPI.DBContext;
using TrackwiseAPI.Models.Entities;
using TrackwiseAPI.Models.Interfaces;
using TrackwiseAPI.Models.Repositories;
using TrackwiseAPI.Models.ViewModels;

namespace TrackwiseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class CustomerController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<AppUser> _claimsPrincipalFactory;
        private readonly IConfiguration _configuration;
        private readonly ICustomerRepository _customerRepository;


        public CustomerController(UserManager<AppUser> userManager,
            IUserClaimsPrincipalFactory<AppUser> claimsPrincipalFactory,
            IConfiguration configuration,
            ICustomerRepository customerRepository)
        {
            _userManager = userManager;
            _claimsPrincipalFactory = claimsPrincipalFactory;
            _configuration = configuration;
            _customerRepository = customerRepository;
        }


        [HttpGet]
        [Route("GetAllCustomers")]
        public async Task<IActionResult> GetAllCustomers()
        {
            try
            {
                var results = await _customerRepository.GetAllCustomerAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }


        [HttpGet]
        [Route("GetCustomer/{customerId}")]
        public async Task<IActionResult> GetCourseAsync(string customerId)
        {
            try
            {
                var result = await _customerRepository.GetCustomerAsync(customerId);

                if (result == null) return NotFound("Customer does not exist. You need to create it first");

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support");
            }
        }

        [HttpPost]
        [Route("AddCustomer")]
        public async Task<IActionResult> AddCustomer(CustomerVM cvm)
        {
            var customerId = Guid.NewGuid().ToString();

            var customer = new Customer { Customer_ID = customerId, Name = cvm.Name, LastName = cvm.LastName, Email = cvm.Email, Password = cvm.Password };

            try
            {
                _customerRepository.Add(customer);
                await _customerRepository.SaveChangesAsync();

                var user = new AppUser
                {
                    Id = customerId,
                    UserName = cvm.Email,
                    Email = cvm.Email
                };

                var result = await _userManager.CreateAsync(user, cvm.Password);

                await _userManager.AddToRoleAsync(user, "Customer");

                if (result.Errors.Count() > 0)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error. Please contact support.");

            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }

            return Ok(customer);
        }

        [HttpPut]
        [Route("EditCustomer/{customerId}")]
        public async Task<ActionResult<CustomerVM>> EditCustomer(string customerId, CustomerVM cvm)
        {
            try
            {
                var existingCustomer = await _customerRepository.GetCustomerAsync(customerId);
                if (existingCustomer == null) 
                    return NotFound($"The customer does not exist");

                var existingUser = await _userManager.FindByIdAsync(customerId);
                if (existingUser == null)
                    return NotFound($"The corresponding user does not exist");

                if (existingCustomer.Name == cvm.Name &&
                    existingCustomer.LastName == cvm.LastName &&
                    existingCustomer.Email == cvm.Email &&
                    existingCustomer.Password == cvm.Password)
                {
                    // No changes made, return the existing driver without updating
                    return Ok(existingCustomer);
                }

                existingCustomer.Name = cvm.Name;
                existingCustomer.LastName = cvm.LastName;
                existingCustomer.Email = cvm.Email;
                existingCustomer.Password = cvm.Password;

                existingUser.UserName = cvm.Email;
                existingUser.Email = cvm.Email;
                await _userManager.RemovePasswordAsync(existingUser);
                await _userManager.AddPasswordAsync(existingUser, cvm.Password);
                existingUser.SecurityStamp = Guid.NewGuid().ToString();

                var customerUpdateResult = await _customerRepository.SaveChangesAsync();
                var userUpdateResult = await _userManager.UpdateAsync(existingUser);

                if (customerUpdateResult && userUpdateResult.Succeeded)
                {
                    return Ok(existingCustomer);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("Your request is invalid.");
        }


        [HttpDelete]
        [Route("DeleteCustomer/{customerId}")]
        public async Task<IActionResult> DeleteCourse(string customerId)
        {
            try
            {
                var existingCustomer = await _customerRepository.GetCustomerAsync(customerId);

                if (existingCustomer == null) 
                    return NotFound($"The course does not exist");

                var user = await _userManager.FindByEmailAsync(existingCustomer.Email);
                if (user != null)
                {
                    var result = await _userManager.DeleteAsync(user);
                    if (!result.Succeeded)
                    {
                        return StatusCode(500, "Failed to delete the associated user.");
                    }
                }

                _customerRepository.Delete(existingCustomer);

                if (await _customerRepository.SaveChangesAsync())
                {
                    return Ok(existingCustomer);
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
