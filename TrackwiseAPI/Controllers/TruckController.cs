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
    public class TruckController : ControllerBase
    {
        private readonly ITruckRepository _truckRepository;

        public TruckController(ITruckRepository truckRepository)
        {
            _truckRepository = truckRepository;
        }

        [HttpGet]
        [Route("GetAllTrucks")]
        public async Task<IActionResult> GetAllTrucks()
        {
            try
            {
                var results = await _truckRepository.GetAllTrucksAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        //Get a specific truck
        [HttpGet]
        [Route("GetTruck/{truckLicense}")]
        public async Task<IActionResult> GetTruckAsync(string truckLicense)
        {
            try
            {
                var result = await _truckRepository.GetTruckAsync(truckLicense);

                if (result == null) return NotFound("Truck does not exist");

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support");
            }
        }

        //Add a truck
        [HttpPost]
        [Route("AddTruck")]
        public async Task<IActionResult> AddTruck(Models.ViewModels.TruckVM tvm)
        {
            var truck = new Truck { Model = tvm.Model };

            try
            {
                _truckRepository.Add(truck);
                await _truckRepository.SaveChangesAsync();

            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }

            return Ok(truck);
        }

        //update truck
        [HttpPut]
        [Route("EditTruck/{truckLicense}")]
        public async Task<ActionResult<Models.ViewModels.TruckVM>> EditTruck(string truckLicense, TruckVM tvm)
        {
            try
            {
                var existingTruck = await _truckRepository.GetTruckAsync(truckLicense);
                if (existingTruck == null) return NotFound($"The truck does not exist");

                existingTruck.Model = tvm.Model;

                if (await _truckRepository.SaveChangesAsync())
                {
                    return Ok(existingTruck);
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
        [Route("DeleteTruck/{truckLicense}")]
        public async Task<IActionResult> DeleteTruck(string truckLicense)
        {
            try
            {
                var existingTruck = await _truckRepository.GetTruckAsync(truckLicense);

                if (existingTruck == null) return NotFound($"The truck does not exist");
                _truckRepository.Delete(existingTruck);

                if (await _truckRepository.SaveChangesAsync()) return Ok(existingTruck);

            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("Your request is invalid.");
        }
    }
}
