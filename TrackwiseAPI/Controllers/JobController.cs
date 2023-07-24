using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
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
        [Route("GetAvailableTrailer/{Type}")]
        public async Task<IActionResult> getTrailerWithTypeForJob(string type)
        {
            try
            {
                var results = await _jobRepository.GetAvailableTrailerWithTypeAsync(type);
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

                return Ok(truckRouteData);
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //If trailer weight > job weight then 1 delivery
        //else if trailer weight < job weight, get kleinste trailer nodig vir 2 deliveries
        [HttpPost]
        [Route("CreateJob")]
        public async Task<IActionResult> CreateJob(JobVM jvm)
        {
            
            //ek kan if insit vir die user wat ingelog is om sy ID te vat en dan die ander null te maak
            var job = new Job
            {
                StartDate = jvm.StartDate,
                DueDate = jvm.DueDate,
                Pickup_Location = jvm.Pickup_Location,
                Dropoff_Location = jvm.Dropoff_Location,
                Total_Weight = jvm.Total_Weight,
                Admin_ID = jvm.Admin_ID,
                Job_Type_ID = jvm.Job_Type_ID
            };

            var drivers = await _jobRepository.GetAvailableDriverAsync();
            var trucks = await _jobRepository.GetAvailableTruckAsync();
            var trailers = await _jobRepository.GetAvailableTrailerWithTypeAsync(job.JobType.Name.ToString());

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

            //Date logic
            double totaltimefortrip = 0;
            double maxHrsPerDay = 14.0; // Maximum driving hours per day
            double driveDuration = 4.0; // Duration of continuous driving (hours)
            double breakDuration = 0.5; // Duration of the break after continuous driving (hours)
                                        
            int numberOfDrivingPeriods = (int)(durationInHrs / (driveDuration + breakDuration));
            double totalDrivingTime = numberOfDrivingPeriods * (driveDuration + breakDuration);
            double remainingHrs = durationInHrs - (numberOfDrivingPeriods * (driveDuration + breakDuration));

            totalDrivingTime += Math.Min(remainingHrs, driveDuration);

            // If the total driving time exceeds the maximum driving hours per day, limit it to the maximum
            totalDrivingTime = Math.Min(totalDrivingTime, maxHrsPerDay);
            totaltimefortrip = totalDrivingTime + (numberOfDrivingPeriods * breakDuration);
            /*
            double JobHrs = 0;
            TimeSpan timeSpan = job.DueDate - job.StartDate;
            JobHrs = (double)timeSpan.TotalHours;
            */

            /*
            function OneDriver
            As hy genoeg tyd het om die weight te cover dan kan een ou dit doen.
            Kry die jobweight. Kyk na trailer weights en bepaal wat die optimal manier is. 
            As trailer weight > jobweight dans dit goed. 
            Anders kry die beste trailer wat gaan pas vir meer as een trip
            */


            double JobHrs = 0;
            double x = 0; int deliveries = 0;
            
            if (drivers.Length>0 && trucks.Length>0 && trailers.Length>0) 
            {
                if (totaltimefortrip < JobHrs)
                {
                    return NotFound("Not enough time for delivery");
                }
                if (job.Total_Weight<30)
                {
                    return NotFound("Delivery weight less than 30");
                }
                if (5>1)
                {
                    //35ton per trailer - > 1200/35 = 34.2857143 trips nodig van 35ton
                    // -> 1280/35 = 36.5714286 trips nodig van 35ton
                    x = job.Total_Weight / 35; 
                    deliveries = (int)x;

                }
            }
            else 
            {
                return NotFound("No available driver/truck/trailer for the job");
            }
            return Ok("lol");


            //Bigtrailer is n trailer wat die job in een delivery kan doen
            //Kan die trailer uit die list kry wat die naaste aan die Job weight is.
            var Bigtrailer = trailers.Where(trailer => trailer.Weight > job.Total_Weight).ToArray();


            //Smalltrailer is n trailer wat die job nie in een delivery kan doen nie
            //Kan die trailers kry wat saam > as die job weight is vir die minste deliveries
            //Ek moet dan besluit wanneer dit een driver is wat dit doen of meer as een driver. (kyk na die tyd wat dit vat)
            var Smalltrailer = trailers.Where(trailer => trailer.Weight < job.Total_Weight).ToArray();
            
            /*
            if (Bigtrailer.Length > 0)
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

                var delivery = new Delivery
                {
                    Delivery_Weight = job.Total_Weight,
                    Driver_ID = driverid,
                    TruckID = truckid,
                    TrailerID = trailerid,
                    Job_ID = job.Job_ID
                };
                _jobRepository.Add(job);
                _jobRepository.Add(delivery);
                await _jobRepository.SaveChangesAsync();
                return Ok(job);

            }
            else
            {
                // Handle the case when no big trailer is available
                return NotFound("No available big trailer found");
            }
            */
            
        }

    }
}
