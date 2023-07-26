using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Globalization;
using TrackwiseAPI.DBContext;
using TrackwiseAPI.Models.BingMapsAPI;
using TrackwiseAPI.Models.Entities;
using TrackwiseAPI.Models.Interfaces;
using TrackwiseAPI.Models.Repositories;
using TrackwiseAPI.Models.ViewModels;

namespace TrackwiseAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {

        private readonly IJobRepository _jobRepository;

        public JobController(IJobRepository jobRepository, TruckRouteService truckRouteService)
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


        [HttpPost]
        [Route("CreateJob")]
        public async Task<IActionResult> CreateJob(JobVM jvm)
        {
            var Job_ID = Guid.NewGuid().ToString();
            var job = new Job
            {
                Job_ID = Job_ID,  
                StartDate = DateTime.ParseExact("2023-07-23 00:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                DueDate = DateTime.ParseExact("2023-07-31 00:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                Pickup_Location = jvm.Pickup_Location,
                Dropoff_Location = jvm.Dropoff_Location,
                Total_Weight = jvm.Total_Weight,
                Admin_ID = jvm.Admin_ID,
                Job_Type_ID = jvm.Job_Type_ID
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


            double JobHrs = 0;
            double x = 0; int deliveries = 0;
            
            if (drivers.Length>0 && trucks.Length>0 && trailers.Length>0) 
            {
                if (totalTravelTime < JobHrs)
                {
                    return NotFound("Not enough time for delivery");
                }
                if (job.Total_Weight < 30)
                {
                    return NotFound("Delivery weight less than 30");
                }
                //35ton per trailer - > 35/35 = 1 trips nodig van 35ton
                // -> 70/35 = 2 trips nodig van 35ton
                x = job.Total_Weight / 35;
                deliveries = (int)x;

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
                        Delivery_Weight = job.Total_Weight,
                        Driver_ID = driverid,
                        TruckID = truckid,
                        TrailerID = trailerid,
                        Job_ID = job.Job_ID
                    };
                    try
                    {
                        _jobRepository.Add(job);
                        await _jobRepository.SaveChangesAsync();
                        _jobRepository.Add(delivery);
                        await _jobRepository.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        return BadRequest("Invalid transaction");
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

                        var deliveries1 = new List<Delivery>();
                        for (int i = 0; i < deliveries; i++)
                        {

                            var Delivery_ID = Guid.NewGuid().ToString();
                            var deliveryObj = new Delivery
                            {
                                Delivery_ID = Delivery_ID,
                                Delivery_Weight = job.Total_Weight/deliveries,
                                Driver_ID = driverId,
                                TruckID = truckId,
                                TrailerID = trailerId,
                                Job_ID = job.Job_ID
                            };

                            deliveries1.Add(deliveryObj);
                        }
                        foreach (var deliveryObj in deliveries1)
                        {
                            _jobRepository.Add(deliveryObj);
                        }

                        _jobRepository.Add(job);
                        await _jobRepository.SaveChangesAsync();
                        return Ok(job);
                    }
                    else if (totalsolotime>JobHrs)
                    {

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
