using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrackwiseAPI.Models.Entities;
using TrackwiseAPI.Models.Interfaces;
using TrackwiseAPI.Models.ViewModels;

namespace TrackwiseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
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
        public async Task<IActionResult> GetCourseAsync(int customerId)
        {
            try
            {
                var result = await _customerRepository.GetCustomerAsync(customerId);

                if (result == null) return NotFound("Course does not exist. You need to create it first");

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support");
            }
        }

        [HttpPost]
        [Route("AddCustomer")]
        public async Task<IActionResult> AddCourse(CustomerVM cvm)
        {
            var customer = new Customer { Name = cvm.Name, LastName = cvm.LastName, Email = cvm.Email, Password = cvm.Password };

            try
            {
                _customerRepository.Add(customer);
                await _customerRepository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }

            return Ok(customer);
        }
        [HttpPut]
        [Route("EditCustomer/{customerId}")]
        public async Task<ActionResult<CustomerVM>> EditCourse(int customerId, CustomerVM customerModel)
        {
            try
            {
                var existingCustomer = await _customerRepository.GetCustomerAsync(customerId);
                if (existingCustomer == null) return NotFound($"The course does not exist");

                if (existingCustomer.Name == customerModel.Name &&
                    existingCustomer.LastName == customerModel.LastName &&
                    existingCustomer.Email == customerModel.Email &&
                    existingCustomer.Password == customerModel.Password)
                {
                    // No changes made, return the existing driver without updating
                    return Ok(existingCustomer);
                }

                existingCustomer.Name = customerModel.Name;
                existingCustomer.LastName = customerModel.LastName;
                existingCustomer.Email = customerModel.Email;
                existingCustomer.Password = customerModel.Password;

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
        [HttpDelete]
        [Route("DeleteCustomer/{customerId}")]
        public async Task<IActionResult> DeleteCourse(int customerId)
        {
            try
            {
                var existingCourse = await _customerRepository.GetCustomerAsync(customerId);

                if (existingCourse == null) return NotFound($"The course does not exist");

                _customerRepository.Delete(existingCourse);

                if (await _customerRepository.SaveChangesAsync()) return Ok(existingCourse);

            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("Your request is invalid.");
        }

    }
}
