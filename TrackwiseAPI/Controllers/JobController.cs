
﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using TrackwiseAPI.DBContext;
using TrackwiseAPI.Models.BingMapsAPI;
using TrackwiseAPI.Models.DataTransferObjects;
using TrackwiseAPI.Models.Email;
using TrackwiseAPI.Models.Entities;
using TrackwiseAPI.Models.Interfaces;
using TrackwiseAPI.Models.Repositories;
using TrackwiseAPI.Models.ViewModels;
using TrackwiseAPI.Controllers;

namespace TrackwiseAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJobRepository _jobRepository;
        private readonly IDriverRepository _driverRepository;
        private readonly ITruckRepository _truckRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly TwDbContext _context;
        private readonly IAuditRepository _auditRepository;
        private readonly MailController _mailController;
        private readonly IBreakIntervalRepository _BreakIntervalRepository;
        private readonly IRestPeriodRepository _restPeriodRepository;
        private readonly IHrsRepository _hrsRepository;

        public JobController(
            IJobRepository jobRepository, 
            IDriverRepository driverRepository,
            ITruckRepository truckRepository,
            TruckRouteService truckRouteService,
            UserManager<AppUser> userManager,
            IWebHostEnvironment hostingEnvironment,
            TwDbContext context,
            IAuditRepository auditRepository,
            MailController mailController,
            IBreakIntervalRepository breakIntervalRepository,
            IRestPeriodRepository restPeriodRepository,
            IHrsRepository hrsRepository)

        {
            _jobRepository = jobRepository;
            _driverRepository = driverRepository;
            _truckRepository = truckRepository;
            _truckRouteService = truckRouteService ?? throw new ArgumentNullException(nameof(truckRouteService));
            _apiKey = "Ah63Z-rLDLN8UftrfVAKYtuQBMSK_EE57L2E7a6NTg5htVdU8gPnn5o7d_Yujc9j"; // Replace this with your actual API key
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            _auditRepository = auditRepository;
            _mailController = mailController;
            _BreakIntervalRepository = breakIntervalRepository;
            _restPeriodRepository = restPeriodRepository;
            _hrsRepository = hrsRepository;
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
        [Route("GetAllClientJobs")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Client")]

        public async Task<IActionResult> GetAllClientJobs()
        {
            try
            {
                // Retrieve the authenticated user's email address
                var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                // Query the customer repository to get the customer ID
                var client = await _userManager.FindByEmailAsync(userEmail);

                if (client == null)
                {
                    return BadRequest("Client not found");
                }

                var clientId = client.Id;

                if (string.IsNullOrEmpty(clientId))
                {
                    return BadRequest("Client ID not found");
                }

                var result = await _jobRepository.GetAllClientJobsAsync(clientId);


                if (result == null) return NotFound("Order does not exist");

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support");
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

        [HttpGet]
        [Route("DeliveryDocuments/{delivery_ID}")]
        public async Task<IActionResult> DeliveryDocuments(string delivery_ID)
        {
            try
            {
                var results = await _jobRepository.GetDocumentsByDeliveryID(delivery_ID);
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }
        public class UpdateActualWeightRequest
        {
            public double actual_Weight { get; set; }
        }

        [HttpGet]
        [Route("GetDelivery/{delivery_ID}")]
        public async Task<IActionResult> GetDeliveryByID(string delivery_ID)
        {
            try
            {
                var results = await _jobRepository.GetDeliveryByID(delivery_ID);
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpPut("Updateweight/{delivery_ID}")]
        public async Task<ActionResult<UpdateActualWeightRequest>> UpdateActualWeight(string delivery_ID, UpdateActualWeightRequest request)
        {
            try
            {
                var delivery = await _jobRepository.GetDeliveryByID(delivery_ID);

                if (delivery == null)
                {
                    return NotFound("Delivery not found");
                }

                delivery.Delivery_Weight = request.actual_Weight;
                delivery.WeightCaptured = true;

                // Save changes to the database
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Actual weight updated successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating actual weight");
            }
        }
    

    [HttpPut]
        [Route("{deliveryId}/status")]
        public async Task<IActionResult> UpdateDeliveryStatus(string deliveryId)
        {
            var delivery = await _context.Deliveries.FirstOrDefaultAsync(d => d.Delivery_ID == deliveryId);

            if (delivery == null)
            {
                return NotFound();
            }

            var deliveryStatusId = "2";
            var newDeliveryStatus = await _context.DeliveryStatuses.FirstOrDefaultAsync(ds => ds.Delivery_Status_ID == deliveryStatusId);
            if (newDeliveryStatus == null)
            {
                // Return a Bad Request or appropriate response if the DeliveryStatus with ID 2 is not found
                return BadRequest("Invalid DeliveryStatus ID");
            }
            // Update the delivery's DeliveryStatus to the new status
            delivery.Delivery_Status_ID = newDeliveryStatus.Delivery_Status_ID;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut]
        [Route("{jobId}/jobstatus")]
        public async Task<IActionResult> UpdatejobStatus(string jobId)
        {
            var job = await _context.Jobs.FirstOrDefaultAsync(d => d.Job_ID == jobId);

            if (job == null)
            {
                return NotFound();
            }

            var jobStatusId = "2";
            var newJobStatus = await _context.JobsStatus.FirstOrDefaultAsync(ds => ds.Job_Status_ID == jobStatusId);
            if (newJobStatus == null)
            {
                // Return a Bad Request or appropriate response if the DeliveryStatus with ID 2 is not found
                return BadRequest("Invalid DeliveryStatus ID");
            }
            // Update the delivery's DeliveryStatus to the new status
            job.Job_Status_ID= newJobStatus.Job_Status_ID;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut]
        [Route("{driverId}/driverstatus")]
        public async Task<IActionResult> UpdatedriverStatus(string driverId)
        {
            var driver = await _context.Drivers.FirstOrDefaultAsync(d => d.Driver_ID == driverId);

            if (driver == null)
            {
                return NotFound();
            }

            var driverStatusId = "1";
            var newdriverStatus = await _context.DriverStatuses.FirstOrDefaultAsync(ds => ds.Driver_Status_ID == driverStatusId);
            if (newdriverStatus == null)
            {
                // Return a Bad Request or appropriate response if the DeliveryStatus with ID 2 is not found
                return BadRequest("Invalid DeliveryStatus ID");
            }
            // Update the delivery's DeliveryStatus to the new status
            driver.Driver_Status_ID = newdriverStatus.Driver_Status_ID;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut]
        [Route("{trailerId}/trailerstatus")]
        public async Task<IActionResult> UpdatetrailerStatus(string trailerId)
        {
            var trailer = await _context.Trailers.FirstOrDefaultAsync(d => d.TrailerID == trailerId);

            if (trailer == null)
            {
                return NotFound();
            }

            var trailerStatusId = "1";
            var newtrailerStatus = await _context.TrailerStatuses.FirstOrDefaultAsync(ds => ds.Trailer_Status_ID == trailerStatusId);
            if (newtrailerStatus == null)
            {
                // Return a Bad Request or appropriate response if the DeliveryStatus with ID 2 is not found
                return BadRequest("Invalid DeliveryStatus ID");
            }
            // Update the delivery's DeliveryStatus to the new status
            trailer.Trailer_Status_ID = newtrailerStatus.Trailer_Status_ID;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut]
        [Route("{truckId}/truckstatus")]
        public async Task<IActionResult> UpdatetruckStatus(string truckId)
        {
            var truck = await _context.Trucks.FirstOrDefaultAsync(d => d.TruckID == truckId);

            if (truck == null)
            {
                return NotFound();
            }

            var truckStatusId = "1";
            var newtruckStatus = await _context.TruckStatuses.FirstOrDefaultAsync(ds => ds.Truck_Status_ID == truckStatusId);
            if (newtruckStatus == null)
            {
                // Return a Bad Request or appropriate response if the DeliveryStatus with ID 2 is not found
                return BadRequest("Invalid DeliveryStatus ID");
            }
            // Update the delivery's DeliveryStatus to the new status
            truck.Truck_Status_ID = newtruckStatus.Truck_Status_ID;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //GetAvailableDrivers
        [HttpPost]
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

                var startLatitude = routeResponse.ResourceSets.FirstOrDefault()?.Resources.FirstOrDefault()?.RouteLegs.FirstOrDefault()?.ActualStart.Coordinates[0];
                var startLongitude = routeResponse.ResourceSets.FirstOrDefault()?.Resources.FirstOrDefault()?.RouteLegs.FirstOrDefault()?.ActualStart.Coordinates[1];
                var endLatitude = routeResponse.ResourceSets.FirstOrDefault()?.Resources.FirstOrDefault()?.RouteLegs.FirstOrDefault()?.ActualEnd.Coordinates[0];
                var endLongitude = routeResponse.ResourceSets.FirstOrDefault()?.Resources.FirstOrDefault()?.RouteLegs.FirstOrDefault()?.ActualEnd.Coordinates[1];
                var startWaypoint = new Location
                {
                    Latitude = startLatitude,
                    Longitude = startLongitude
                };
                var endWaypoint = new Location
                {
                    Latitude = endLatitude,
                    Longitude = endLongitude
                };
                var waypoints = new List<Location> { startWaypoint, endWaypoint };
                string staticMapUrl = StaticMapService.GetStaticMapUrl(waypoints, _apiKey);

                // Now you can access the data in the deserialized object
                var distance = routeResponse.ResourceSets.FirstOrDefault()?.Resources.FirstOrDefault()?.RouteLegs.FirstOrDefault()?.TravelDistance;
                var duration = routeResponse.ResourceSets.FirstOrDefault()?.Resources.FirstOrDefault()?.RouteLegs.FirstOrDefault()?.TravelDuration;

                var truckRouteInfo = new TruckRouteInfo
                {
                    Mapurl = staticMapUrl,
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
                Delivery_Status_ID = delivery.Delivery_Status_ID,
                TrailerID = delivery.TrailerID,
                TruckID = delivery.TruckID,
                
                Jobs = new JobDTO
                {
                    Job_ID = delivery.Jobs.Job_ID,
                    StartDate = delivery.Jobs.StartDate,
                    DueDate = delivery.Jobs.DueDate,
                    PickupLocation = delivery.Jobs.PickupLocation,
                    DropoffLocation = delivery.Jobs.DropoffLocation,
                    type = delivery.Jobs.type,
                    mapURL = delivery.Jobs.mapURL,

                }
            }).ToArray();
            return Ok(deliveryDTOs);
        }

        [HttpPost]
        [Route("AddDocuments")]
        public async Task<IActionResult> AddDocuments(DocumentVM documentVM)
        {
            try
            {
                var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var auditId = Guid.NewGuid().ToString();
                var audit = new Audit { Audit_ID = auditId, Action = "Upload documents", CreatedDate = DateTime.Now, User = userEmail };
                var documents = new DocumentVM
                {
                    Documents = documentVM.Documents.Select(ol => new DocumentDTO
                    {
                        Document_ID = ol.Document_ID,
                        Image = ol.Image,
                        DocType = ol.DocType,
                        Delivery_ID = ol.Delivery_ID
                    }).ToList()
                };

                foreach (var document in documents.Documents)
                {
                    // Create a new Document entity
                    var document_ID = Guid.NewGuid().ToString();
                    var newDocument = new Document
                    {
                        Document_ID = document_ID,
                        Image = document.Image,
                        DocType = document.DocType,
                        Delivery_ID = document.Delivery_ID,
                    };

                    // Add the new document to the context (not saving to the database yet)
                    _context.Documents.Add(newDocument);
                }

                // Save all changes to the database at once
                await _jobRepository.SaveChangesAsync();
                _auditRepository.Add(audit);
                await _auditRepository.SaveChangesAsync();

                return Ok("Documents added successfully");
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }
        }



        public class UpdateFuel
        {
            public double? Total_Fuel { get; set; }
        }

        //Fuel
        //[HttpGet]
        //[Route("GetFuel/{delivery_ID}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]

        //public async Task<IActionResult> GetFuelAsync(string delivery_ID)
        //{
        //    try
        //    {
        //        var result = await _jobRepository.GetDeliveryByID(delivery_ID);

        //        if (result == null) return NotFound("Job does not exist");

        //        return Ok(result);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500, "Internal Server Error. Please contact support");
        //    }
        //}


        [HttpPut]
        [Route("EditFuel/{delivery_ID}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> EditFuel(string delivery_ID, UpdateFuel request)
        {
            try
            {
                var delivery = await _jobRepository.GetDeliveryByID(delivery_ID);

                delivery.TotalFuel = request.Total_Fuel;
                await _context.SaveChangesAsync();


                return Ok(new { Message = "Total fuel updated successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating fuel");
            }

        }


        public class UpdateMileageFuel
        {
            public double? Initial_Mileage { get; set; }
            public double? Final_Mileage { get; set; }
            public double? Total_Fuel { get; set; }
            //public string TruckID { get; set; }
        }

        //Mileage
        [HttpGet]
        [Route("GetAllMileageFuel")]
        public async Task<IActionResult> GetAllMileageFuel()
        {
            try
            {
                var results = await _jobRepository.GetAllMileageFuelAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }


        [HttpGet]
        [Route("GetMileageFuel/{delivery_ID}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]

        public async Task<IActionResult> GetMileageAsync(string delivery_ID)
        {
            try
            {
                var result = await _jobRepository.GetDeliveryByID(delivery_ID);

                if (result == null) return NotFound("Job does not exist");

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support");
            }
        }

        
        [HttpPut]
        [Route("EditMileageFuel/{delivery_ID}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> EditMileage(string delivery_ID, UpdateMileageFuel request)
        {
            try
            {
                var delivery = await _jobRepository.GetDeliveryByID(delivery_ID);
                var truck = await _truckRepository.GetTruckAsync(delivery.TruckID);

                delivery.Initial_Mileage = request.Initial_Mileage;
                delivery.Final_Mileage = request.Final_Mileage;
                delivery.MileageCaptured = true;

                truck.Mileage = request.Final_Mileage;
                await _context.SaveChangesAsync();


                return Ok(new { Message = "Actual mileage updated successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating mileage");
            }

        }


        [HttpPost]
        [Route("CreateJob")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Client")]
        public async Task<IActionResult> CreateJob(JobVM jvm)
        {
            // Retrieve the authenticated user's email address
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var auditId = Guid.NewGuid().ToString();
            var audit = new Audit { Audit_ID = auditId, Action = "Create Job", CreatedDate = DateTime.Now, User = userEmail };

            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null) {return BadRequest("User not found");}
            var userId = user.Id;
            if (string.IsNullOrEmpty(userId)) {return BadRequest("User ID not found");}

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
                Job_Status_ID = "1",
                Map ="",
                DeliveryCount = 0,
                CompletedDeliveries = 0,
            };

            var trailers = await _jobRepository.GetAvailableTrailerWithTypeAsync(job.Job_Type_ID);
            var drivers = await _jobRepository.GetAvailableDriverAsync();
            var trucks = await _jobRepository.GetAvailableTruckAsync();

            var truckRouteResult = await CalculateTruckRoute(job.Pickup_Location, job.Dropoff_Location);
            string mapurl;
            double distanceInKm = 0; // Declare the distance variable with a default value
            double durationInHrs = 0; // Declare the duration variable with a default value
            if (truckRouteResult is OkObjectResult okObjectResult)
            {
                var truckRouteInfo = okObjectResult.Value as TruckRouteInfo;

                // Now you can access the properties of truckRouteInfo like distance and duration
                mapurl = truckRouteInfo.Mapurl;
                job.Map = mapurl;
                distanceInKm = truckRouteInfo.Distance;
                durationInHrs = truckRouteInfo.Duration;
            }



            // durationInHrs = 19;
            
            var result = await _BreakIntervalRepository.GetBreakAsync();
            double breakInterval = result.Break_Amount;

            var result2 = await _restPeriodRepository.GetRestAsync();
            double restDuration = result2.Rest_Amount;

            var result3 = await _hrsRepository.GetHrsAsync();
            double maxHrsPerDay = result3.Hrs_Amount;


            double numBreaks = Math.Floor(durationInHrs / breakInterval); //4
            double totalRestTime = numBreaks * restDuration; //2
            double totalTravelTime = durationInHrs + totalRestTime; //21

            if (totalTravelTime > maxHrsPerDay) //21>14
            {
                totalTravelTime += 8.0; //21+8 = 29hrs na die location toe.
            }


            //double JobHrs = 16.00; //dis die ure wat jy het om die job te doen.
            var start = job.StartDate;
            var due = job.DueDate;
            TimeSpan timeSpan = due - start;
            double JobHrs = timeSpan.TotalHours;

            double x = 0; int deliveries = 0; double y = 0;
            double jobweight = 0;
            double deliveryweight = 0;
            double remainingweight = 0;

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

                job.DeliveryCount = deliveries;

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
                        Job_ID = job.Job_ID,
                        Delivery_Status_ID = "1",
                        TotalFuel = 0,
                        WeightCaptured = false,
                        MileageCaptured = false
                    };
                    var number = driver.PhoneNumber;
                    var newnumber = "+27" + number.Substring(1);
                    var mailContent = new MessageModel { Body = "You have a new Job with one delivery. View more details on your driver app",ToPhoneNumber = newnumber };
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
                            var mail = await _mailController.SendMessage(mailContent);
                            await _jobRepository.SaveChangesAsync();
                            _auditRepository.Add(audit);
                            await _auditRepository.SaveChangesAsync();
                           
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

                        deliveryweight = job.Total_Weight;
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
                                Job_ID = job.Job_ID,
                                Delivery_Status_ID = "1",
                                TotalFuel = 0,
                                WeightCaptured = false,
                                MileageCaptured = false
                            };
                            deliveries1.Add(deliveryObj);

                            deliveryweight -= jobweight;  //100 - 35 = 65   || 65 - 35 = 30
                            if((deliveryweight < 35) && (deliveryweight >= 30))
                            {
                                jobweight = deliveryweight;
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
                                _auditRepository.Add(audit);
                                await _auditRepository.SaveChangesAsync();
                                await _jobRepository.SaveChangesAsync();
                                transaction.Commit(); // Commit the transaction if everything is successful
                                                      // Send a message to the driver
                                var number = driver.PhoneNumber;
                                var newnumber = "+27" + number.Substring(1);
                                var mailContent = new MessageModel
                                {
                                    Body = $"You have a new Job with {deliveries1.Count} deliveries.",
                                    ToPhoneNumber = newnumber
                                };
                                var mail = await _mailController.SendMessage(mailContent);
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
                        deliveryweight = job.Total_Weight;
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
                                Job_ID = job.Job_ID,
                                Delivery_Status_ID = "1",
                                TotalFuel = 0,
                                WeightCaptured = false,
                                MileageCaptured = false
                            };

                            allDeliveries.Add(deliveryObj);
                            deliveryweight -= jobweight;  //100 - 35 = 65   || 65 - 35 = 30
                            if ((deliveryweight < 35) && (deliveryweight >= 30))
                            {
                                jobweight = deliveryweight;
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
                                _auditRepository.Add(audit);
                                await _auditRepository.SaveChangesAsync();
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

        [HttpPut]
        [Route("CompleteDelivery/{delivery_ID}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Driver")]
        public async Task<IActionResult> CompleteDelivery(string delivery_ID)
        {
            try
            {
                var delivery = await _jobRepository.GetDeliveryByID(delivery_ID);
                var job = await _jobRepository.GetJobAsync(delivery.Job_ID);

                delivery.Delivery_Status_ID = "2";
                await _jobRepository.SaveChangesAsync();

                job.CompletedDeliveries += 1;

                // Retrieve the authenticated user's email address
                var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var auditId = Guid.NewGuid().ToString();
                var audit = new Audit { Audit_ID = auditId, Action = "Driver Delivery Completed", CreatedDate = DateTime.Now, User = userEmail };
                // Query the customer repository to get the customer ID
                var user = await _userManager.FindByEmailAsync(userEmail);

                if (user == null)
                {
                    return BadRequest("Driver not found");
                }

                var userId = user.Id;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("Driver ID not found");
                }

                var driver = await _driverRepository.GetDriverAsync(userId);

                var driverDeliveries = await _jobRepository.GetDriverDeliveriesAsync(userId);

                if (driverDeliveries.Length == 0)
                {
                    driver.Driver_Status_ID = "1";
                    delivery.Truck.Truck_Status_ID = "1";
                    delivery.Trailer.Trailer_Status_ID = "1";
                }

                if (job.CompletedDeliveries == job.DeliveryCount)
                {
                    job.Job_Status_ID = "2";
                }

                if (await _jobRepository.SaveChangesAsync())
                {
                    _auditRepository.Add(audit);
                    await _auditRepository.SaveChangesAsync();
                    return Ok(delivery);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("Your request is invalid.");
        }

        [HttpPut]
        [Route("CompleteJob/{Job_ID}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> CompleteJob(string Job_ID)
        {
            try
            {
                var job = await _jobRepository.GetJobAsync(Job_ID);
                var capturedCount = 0;
                var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var auditId = Guid.NewGuid().ToString();
                var audit = new Audit { Audit_ID = auditId, Action = "Job Completed", CreatedDate = DateTime.Now, User = userEmail };

                foreach (var delivery in job.Deliveries)
                {
                    if (delivery.MileageCaptured == true && delivery.WeightCaptured == true)
                    {
                        capturedCount++;
                    }
                }

                if (capturedCount == job.DeliveryCount)
                {
                    job.Job_Status_ID = "4";
                }

                if (await _jobRepository.SaveChangesAsync())
                {
                    _auditRepository.Add(audit);
                    await _auditRepository.SaveChangesAsync();
                    return Ok();
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("Your request is invalid.");
        }

        [HttpPut]
        [Route("CancelJob/{Job_ID}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> CancelJob(string Job_ID)
        {
            try
            {
                var job = await _jobRepository.GetJobAsync(Job_ID);

                foreach (var delivery in job.Deliveries)
                {

                    delivery.Delivery_Status_ID = "3";
                    var driver = await _driverRepository.GetDriverAsync(delivery.Driver_ID);
                    driver.Driver_Status_ID = "1";
                    delivery.Truck.Truck_Status_ID = "1";
                    delivery.Trailer.Trailer_Status_ID = "1";
                }

                job.Job_Status_ID = "3";

                if (await _jobRepository.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("Your request is invalid.");
        }

        [HttpGet]
        [Route("GetBreakInterval")]
        public async Task<IActionResult> GetBreak()
        {
            try
            {
                var results = await _BreakIntervalRepository.GetBreakAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpPut]
        [Route("UpdateBreakInterval/{updatedBreak}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> UpdateVAT(double updatedBreak)
        {
            try
            {
                var VAT = await _BreakIntervalRepository.GetBreakAsync();
                if (VAT == null)
                {
                    return NotFound(); // Handle VAT record not found scenario
                }

                VAT.Break_Amount = updatedBreak;

                _BreakIntervalRepository.Update(VAT);

                await _BreakIntervalRepository.SaveChangesAsync();
                return Ok(VAT);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine(ex);

                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("GetRestperiod")]
        public async Task<IActionResult> GetRest()
        {
            try
            {
                var results = await _restPeriodRepository.GetRestAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpPut]
        [Route("UpdateRestPeriod/{updatedRest}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> UpdateRest(double updatedRest)
        {
            try
            {
                var VAT = await _restPeriodRepository.GetRestAsync();
                if (VAT == null)
                {
                    return NotFound(); // Handle VAT record not found scenario
                }

                VAT.Rest_Amount = updatedRest;

                _restPeriodRepository.Update(VAT);

                await _restPeriodRepository.SaveChangesAsync();
                return Ok(VAT);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine(ex);

                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("GetMaxHrs")]
        public async Task<IActionResult> GetHrs()
        {
            try
            {
                var results = await _hrsRepository.GetHrsAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpPut]
        [Route("UpdateMaxHrs/{updatedHrs}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> UpdateHrs(double updatedHrs)
        {
            try
            {
                var VAT = await _hrsRepository.GetHrsAsync();
                if (VAT == null)
                {
                    return NotFound(); // Handle VAT record not found scenario
                }

                VAT.Hrs_Amount = updatedHrs;

                _hrsRepository.Update(VAT);

                await _hrsRepository.SaveChangesAsync();
                return Ok(VAT);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine(ex);

                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

    }
}
