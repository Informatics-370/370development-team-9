
﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;
using TrackwiseAPI.DBContext;
using TrackwiseAPI.Models.BingMapsAPI;
using TrackwiseAPI.Models.DataTransferObjects;
using TrackwiseAPI.Models.Entities;
using TrackwiseAPI.Models.Interfaces;
using TrackwiseAPI.Models.Repositories;
using TrackwiseAPI.Models.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace TrackwiseAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJobRepository _jobRepository;


        public JobController(
            IJobRepository jobRepository, 
            TruckRouteService truckRouteService,
            UserManager<AppUser> userManager
            )

        {
            _jobRepository = jobRepository;
            _truckRouteService = truckRouteService ?? throw new ArgumentNullException(nameof(truckRouteService));
            _apiKey = "Ah63Z-rLDLN8UftrfVAKYtuQBMSK_EE57L2E7a6NTg5htVdU8gPnn5o7d_Yujc9j"; // Replace this with your actual API key
        }

        private readonly TruckRouteService _truckRouteService;
        private readonly string _apiKey; // Store your API key here

        [HttpGet]
        [Route("GetAllAdminJobs")]
        public async Task<IActionResult> GetAllDrivers()
        {
            try
            {
                var results = await _jobRepository.GetAllAdminJobsAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("GetJob/{Job_ID}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Client")]

        public async Task<IActionResult> GetJobAsync(string Job_ID)
        {
            try
            {
                var result = await _jobRepository.GetJobAsync(Job_ID);

                if (result == null) return NotFound("Job does not exist");

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support");
            }
        }

        //GetAvailableTrucks
        [HttpGet]
        [Route("GetAvailableTrucks")]
        public async Task<IActionResult> getTruckForJob()
        {
            try
            {
                var results = await _jobRepository.GetAvailableTruckAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        //GetAvailableTrailersWithTheirTypes
        [HttpGet]
        [Route("GetAvailableTrailer")]
        public async Task<IActionResult> getTrailerWithTypeForJob(string Type)
        {
            try
            {
                var results = await _jobRepository.GetAvailableTrailerWithTypeAsync(Type);
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }


        //GetAvailableDrivers
        [HttpGet]
        [Route("GetAvailableDrivers")]
        public async Task<IActionResult> getDriverForJob()
        {
            try
            {
                var results = await _jobRepository.GetAvailableDriverAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet("calculate")]
        public async Task<IActionResult> CalculateTruckRoute(string startLocation, string endLocation)
        {
            try
            {
                var truckRouteData = await _truckRouteService.CalculateTruckRouteAsync(startLocation, endLocation, _apiKey);

                // Deserialize the JSON response into your RouteResponse model
                var routeResponse = JsonConvert.DeserializeObject<RouteResponse>(truckRouteData);

                // Now you can access the data in the deserialized object
                var distance = routeResponse.ResourceSets.FirstOrDefault()?.Resources.FirstOrDefault()?.RouteLegs.FirstOrDefault()?.TravelDistance;
                var duration = routeResponse.ResourceSets.FirstOrDefault()?.Resources.FirstOrDefault()?.RouteLegs.FirstOrDefault()?.TravelDuration;

                var truckRouteInfo = new TruckRouteInfo
                {
                    Distance = (double)(distance * 1.60934), //km
                    Duration = (double)(duration / 3600)     //hrs

                };

                return Ok(truckRouteInfo);
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetDriverDeliveries")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Driver")]
        public async Task<IActionResult> getDriverDeliveries()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var driver = await _userManager.FindByEmailAsync(userEmail);

            if (driver == null)
            {
                return BadRequest("Driver not found");
            }
            var driverId = driver.Id;
            if (string.IsNullOrEmpty(driverId))
            {
                return BadRequest("Driver ID not found");
            }

            var result = await _jobRepository.GetDriverDeliveriesAsync(driverId);
            var deliveryDTOs = result.Select(delivery => new DeliveryDTO
            {
                Delivery_ID = delivery.Delivery_ID,
                Delivery_Weight = delivery.Delivery_Weight,
                Driver_ID = driverId,
                Jobs = new JobDTO
                {
                    Job_ID = delivery.Jobs.Job_ID,
                    StartDate = delivery.Jobs.StartDate,
                    DueDate = delivery.Jobs.DueDate,
                    PickupLocation = delivery.Jobs.PickupLocation,
                    DropoffLocation = delivery.Jobs.DropoffLocation,
                    type = delivery.Jobs.type

                }
            }).ToArray();
            return Ok(deliveryDTOs);
        }


        [HttpPost]
        [Route("CreateJob")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Client")]
        public async Task<IActionResult> CreateJob(JobVM jvm)
        {
            // Retrieve the authenticated user's email address
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            // Query the customer repository to get the customer ID
            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            var userId = user.Id;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID not found");
            }


            var Job_ID = Guid.NewGuid().ToString();

            var job = new Job
            {
                Job_ID = Job_ID,  
                StartDate = jvm.StartDate,
                DueDate = jvm.DueDate,
                Pickup_Location = jvm.Pickup_Location,
                Dropoff_Location = jvm.Dropoff_Location,
                Total_Weight = jvm.Total_Weight,
                Creator_ID = userId,
                Job_Type_ID = jvm.Job_Type_ID,
                Job_Status_ID = "1"
            };


            var trailers = await _jobRepository.GetAvailableTrailerWithTypeAsync(job.Job_Type_ID);
            var drivers = await _jobRepository.GetAvailableDriverAsync();
            var trucks = await _jobRepository.GetAvailableTruckAsync();

            var truckRouteResult = await CalculateTruckRoute(job.Pickup_Location, job.Dropoff_Location);
            double distanceInKm = 0; // Declare the distance variable with a default value
            double durationInHrs = 0; // Declare the duration variable with a default value
            if (truckRouteResult is OkObjectResult okObjectResult)
            {
                var truckRouteInfo = okObjectResult.Value as TruckRouteInfo;

                // Now you can access the properties of truckRouteInfo like distance and duration
                distanceInKm = truckRouteInfo.Distance;
                durationInHrs = truckRouteInfo.Duration;
            }

            // durationInHrs = 19;
            double breakInterval = 4.0;
            double restDuration = 0.5;
            double maxHrsPerDay = 14.0;

            double numBreaks = Math.Floor(durationInHrs / breakInterval); //4
            double totalRestTime = numBreaks * restDuration; //2
            double totalTravelTime = durationInHrs + totalRestTime; //21

            if (totalTravelTime > maxHrsPerDay) //21>14
            {
                totalTravelTime += 8.0; //21+8 = 29hrs na die location toe.
            }


            double JobHrs = 16.00; //dis die ure wat jy het om die job te doen.
            double x = 0; int deliveries = 0; double y = 0;
            double jobweight = 0;
            double deliveryweight = 0;
            double remainingweight = 0;
            double piele = 0;

            if (drivers.Length>0 && trucks.Length>0 && trailers.Length>0) 
            {
                if (JobHrs<totalTravelTime)
                {
                    return NotFound("Not enough time for delivery");
                }
                if (job.Total_Weight < 30)
                {
                    return NotFound("Delivery weight less than 30");
                }

                x = job.Total_Weight / 35; //   100/35 = 2.8 -> 2dels van 35 
                deliveries = (int)x;

                jobweight = (job.Total_Weight - (job.Total_Weight % 35))/deliveries; //35
                remainingweight = job.Total_Weight - (deliveries * 35); // 30
                if (remainingweight >= 30)
                {
                    deliveries++; //plus delivery vir die laaste 30+ ton
                }

                if (deliveries == 1)
                {
                    // DRIVER
                    var driver = drivers.FirstOrDefault();
                    if (driver == null)
                    {
                        // Handle the case when no driver is available
                        return NotFound("No available driver found");
                    }
                    var driverid = driver.Driver_ID;

                    // TRUCK
                    var truck = trucks.FirstOrDefault();
                    if (truck == null)
                    {
                        // Handle the case when no truck is available
                        return NotFound("No available truck found");
                    }
                    var truckid = truck.TruckID;

                    // TRAILER
                    var trailer = trailers.FirstOrDefault();
                    if (trailer == null)
                    {
                        // Handle the case when no trailer is available
                        return NotFound("No available trailer found");
                    }
                    var trailerid = trailer.TrailerID;

                    var Delivery_ID = Guid.NewGuid().ToString();
                    var delivery = new Delivery
                    {
                        Delivery_ID = Delivery_ID,
                        Delivery_Weight = jobweight,
                        Driver_ID = driverid,
                        TruckID = truckid,
                        TrailerID = trailerid,
                        Job_ID = job.Job_ID
                    };
                    using (var transaction = _jobRepository.BeginTransaction()) // Begin the database transaction
                    {
                        try
                        {
                            _jobRepository.Add(job);
                            _jobRepository.Add(delivery);
                            //update statusses of the users
                            driver.Driver_Status_ID = "2";
                            truck.Truck_Status_ID = "2";
                            trailer.Trailer_Status_ID = "2";

                            await _jobRepository.SaveChangesAsync();
                            transaction.Commit(); // Commit the transaction if everything is successful
                        }
                        catch (Exception)
                        {
                            transaction.Rollback(); // Rollback the transaction if an exception occurs
                            return BadRequest("Invalid transaction");
                        }
                    }
                    return Ok(job);
                }
                if (deliveries>1)
                {
                    double totalsolotime = totalTravelTime * ((deliveries * 2) - 1); //hrs needed for all deliveries by 1 driver
                    if (totalsolotime < JobHrs)
                    {
                        // DRIVER
                        var driver = drivers.FirstOrDefault();
                        if (driver == null)
                        {
                            // Handle the case when no driver is available
                            return NotFound("No available driver found");
                        }
                        var driverId = driver.Driver_ID;

                        // TRUCK
                        var truck = trucks.FirstOrDefault();
                        if (truck == null)
                        {
                            // Handle the case when no truck is available
                            return NotFound("No available truck found");
                        }
                        var truckId = truck.TruckID;

                        // TRAILER
                        var trailer = trailers.FirstOrDefault();
                        if (trailer == null)
                        {
                            // Handle the case when no trailer is available
                            return NotFound("No available trailer found");
                        }
                        var trailerId = trailer.TrailerID;

                        piele = job.Total_Weight;
                        var deliveries1 = new List<Delivery>();
                        for (int i = 0; i < deliveries; i++)
                        {
                            var Delivery_ID = Guid.NewGuid().ToString();
                            var deliveryObj = new Delivery
                            {
                                
                                Delivery_ID = Delivery_ID,
                                Delivery_Weight = jobweight,
                                Driver_ID = driverId,
                                TruckID = truckId,
                                TrailerID = trailerId,
                                Job_ID = job.Job_ID
                            };
                            deliveries1.Add(deliveryObj);
                            
                            piele -= jobweight;  //100 - 35 = 65   || 65 - 35 = 30
                            if((piele < 35) && (piele >= 30))
                            {
                                jobweight = piele;
                            }
                        }
                        using (var transaction = _jobRepository.BeginTransaction())
                        {
                            try
                            {
                                _jobRepository.Add(job);
                                foreach (var deliveryObj in deliveries1)
                                {
                                    _jobRepository.Add(deliveryObj);
                                }
                                // Update the statusses
                                driver.Driver_Status_ID = "2";
                                truck.Truck_Status_ID = "2";
                                trailer.Trailer_Status_ID = "2";

                                // Save changes to the database
                                await _jobRepository.SaveChangesAsync();
                                transaction.Commit(); // Commit the transaction if everything is successful
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback(); // Rollback the transaction if an exception occurs
                                return BadRequest("Invalid transaction");
                            }
                        }
                        return Ok(job);
                    }
                    else // Multiple drivers are required for all deliveries
                    {
                        int driversNeeded = (int)Math.Ceiling(totalsolotime / JobHrs);
                        int trucksNeeded = driversNeeded; // Each driver must have a truck
                        int trailersNeeded = driversNeeded; // Each driver must have a trailer

                        // Check if we have enough available drivers, trucks, and trailers
                        if (drivers.Length < driversNeeded || trucks.Length < trucksNeeded || trailers.Length < trailersNeeded)
                        {
                            return NotFound("Not enough available drivers/trucks/trailers for all deliveries");
                        }
                        piele = job.Total_Weight;
                        List<Delivery> allDeliveries = new List<Delivery>();
                        for (int i = 0; i < deliveries; i++)
                        {
                            // DRIVER
                            var driver = drivers[i % drivers.Length]; // Distribute deliveries among available drivers
                            var driverId = driver.Driver_ID;

                            // TRUCK
                            var truck = trucks[i % trucks.Length]; // Distribute deliveries among available trucks
                            var truckId = truck.TruckID;

                            // TRAILER
                            var trailer = trailers[i % trailers.Length]; // Distribute deliveries among available trailers
                            var trailerId = trailer.TrailerID;

                            var Delivery_ID = Guid.NewGuid().ToString();
                            var deliveryObj = new Delivery
                            {
                                Delivery_ID = Delivery_ID,
                                Delivery_Weight = jobweight,
                                Driver_ID = driverId,
                                TruckID = truckId,
                                TrailerID = trailerId,
                                Job_ID = job.Job_ID
                            };

                            allDeliveries.Add(deliveryObj);
                            piele -= jobweight;  //100 - 35 = 65   || 65 - 35 = 30
                            if ((piele < 35) && (piele >= 30))
                            {
                                jobweight = piele;
                            }
                        }

                        Driver driverToUpdate = null;
                        Truck truckToUpdate = null;
                        Trailer trailerToUpdate = null;

                        using (var transaction = _jobRepository.BeginTransaction())
                        {
                            try
                            {
                                foreach (var deliveryObj in allDeliveries)
                                {
                                    _jobRepository.Add(deliveryObj);
                                }

                                _jobRepository.Add(job);

                                // Update the statuses
                                driverToUpdate = drivers.FirstOrDefault();
                                truckToUpdate = trucks.FirstOrDefault();
                                trailerToUpdate = trailers.FirstOrDefault();

                                if (driverToUpdate != null) driverToUpdate.Driver_Status_ID = "2";
                                if (truckToUpdate != null) truckToUpdate.Truck_Status_ID = "2";
                                if (trailerToUpdate != null) trailerToUpdate.Trailer_Status_ID = "2";

                                // Save changes to the database
                                await _jobRepository.SaveChangesAsync();
                                transaction.Commit(); // Commit the transaction if everything is successful
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback(); // Rollback the transaction if an exception occurs

                                // Log the exception details
                                Console.WriteLine(ex.Message);
                                Console.WriteLine(ex.StackTrace);
                                Console.WriteLine(ex.InnerException?.Message);
                                Console.WriteLine(ex.InnerException?.StackTrace);

                                return BadRequest("Invalid transaction");
                            }
                        }

                        return Ok(job);
                    }
                }

            }
            else 
            {
                return NotFound("No available driver/truck/trailer for the job");
            }
            return Ok("lol");


        }

    }
}
