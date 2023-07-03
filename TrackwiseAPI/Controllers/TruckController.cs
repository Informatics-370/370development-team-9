using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TrackwiseAPI.Models.Entities;
using TrackwiseAPI.Models.Interfaces;
using TrackwiseAPI.Models.Repositories;
using TrackwiseAPI.Models.ViewModels;

namespace TrackwiseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Customer")]
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
        [Route("GetTruck/{TruckID}")]
        public async Task<IActionResult> GetTruckAsync(string TruckID)
        {
            try
            {
                var result = await _truckRepository.GetTruckAsync(TruckID);

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
        public async Task<IActionResult> AddTruck(TruckVM tvm)
        {
            var truck = new Truck { Truck_License = tvm.Truck_License, Model = tvm.Model, Truck_Status_ID = tvm.Truck_Status_ID};

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
        [Route("EditTruck/{TruckID}")]
        public async Task<ActionResult<TruckVM>> EditTruck(string TruckID, TruckVM tvm)
        {
            try
            {
                var existingTruck = await _truckRepository.GetTruckAsync(TruckID);
                if (existingTruck == null) return NotFound($"The truck does not exist");

                if (existingTruck.Truck_License == tvm.Truck_License &&
                    existingTruck.Model == tvm.Model &&
                    existingTruck.Truck_Status_ID == tvm.Truck_Status_ID)
                {
                    // No changes made, return the existing driver without updating
                    return Ok(existingTruck);
                }

                existingTruck.Truck_License = tvm.Truck_License;
                existingTruck.Model = tvm.Model;
                existingTruck.Truck_Status_ID = tvm.Truck_Status_ID;
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
        [Route("DeleteTruck/{TruckID}")]
        public async Task<IActionResult> DeleteTruck(string TruckID)
        {
            try
            {
                var existingTruck = await _truckRepository.GetTruckAsync(TruckID);

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
