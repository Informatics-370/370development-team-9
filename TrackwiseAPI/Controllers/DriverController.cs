using Microsoft.AspNetCore.Mvc;
using TrackwiseAPI.Models.Entities;
using TrackwiseAPI.Models.Interfaces;
using TrackwiseAPI.Models.ViewModels;

namespace TrackwiseAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IDriverRepository _driverRepository;

        public DriverController(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
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
        public async Task<IActionResult> GetTruckAsync(int Driver_ID)
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
            var driver = new Driver { Name = dvm.Name, Lastname = dvm.Lastname, PhoneNumber = dvm.PhoneNumber, Driver_Status_ID = dvm.Driver_Status_ID };

            try
            {
                _driverRepository.Add(driver);
                await _driverRepository.SaveChangesAsync();

            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }

            return Ok(driver);
        }

        //update truck
        [HttpPut]
        [Route("EditDriver/{Driver_ID}")]
        public async Task<ActionResult<DriverVM>> EditDriver(int Driver_ID, DriverVM dvm)
        {

            try
            {
                var existingDriver = await _driverRepository.GetDriverAsync(Driver_ID);

                if (existingDriver == null) return NotFound($"The driver does not exist");
                existingDriver.Name = dvm.Name;
                existingDriver.Lastname = dvm.Lastname;
                existingDriver.PhoneNumber = dvm.PhoneNumber;
                existingDriver.Driver_Status_ID = dvm.Driver_Status_ID;

                if (await _driverRepository.SaveChangesAsync())
                {
                    return Ok(existingDriver);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("Your request is invalid.");
        }

        //Remove truck
        [HttpDelete]
        [Route("DeleteDriver/{Driver_ID}")]
        public async Task<IActionResult> DeleteTruck(int Driver_ID)
        {
            try
            {
                var existingTruck = await _driverRepository.GetDriverAsync(Driver_ID);

                if (existingTruck == null) return NotFound($"The driver does not exist");
                _driverRepository.Delete(existingTruck);

                if (await _driverRepository.SaveChangesAsync()) return Ok(existingTruck);

            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("Your request is invalid.");
        }
    }

}
