using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using TrackwiseAPI.DBContext;
using TrackwiseAPI.Models.DataTransferObjects;
using TrackwiseAPI.Models.Email;
using TrackwiseAPI.Models.Interfaces;
using TrackwiseAPI.Models.Repositories;

namespace TrackwiseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportRepository _reportRepository;
        private readonly ITruckRepository _truckRepository;
        private readonly IJobRepository _jobRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly IDriverRepository _driverRepository;

        public ReportController(
            IReportRepository reportRepository, 
            ITruckRepository truckRepository,
            IJobRepository jobRepository,
            IAdminRepository adminRepository,
            IDriverRepository driverRepository)
        {
            _reportRepository = reportRepository;
            _truckRepository = truckRepository;
            _jobRepository = jobRepository;
            _adminRepository = adminRepository;
            _driverRepository = driverRepository;
        }

        [HttpGet]
        [Route("GetCompleteJobs")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetCompleteJobs()
        {
            try
            {
                var results = await _reportRepository.GetCompleteJobsAsync();
                var loadsDTO = results.Select(loads => new LoadsDTO
                {
                    Job_ID = loads.Job_ID,
                    StartDate = loads.StartDate,
                    DueDate = loads.DueDate,
                    PickupLocation = loads.Pickup_Location,
                    DropoffLocation = loads.Dropoff_Location,
                    type = loads.Job_Type_ID,
                    Weight = loads.Total_Weight,
                    Creator_ID = loads.Creator_ID,
                    Job_Status_ID = loads.Job_Status_ID,
                    JobStatus = new JobStatusDTO
                    {
                        Job_Status_ID = loads.Job_Status_ID,
                        Name = loads.JobStatus.Name,
                        Description = loads.JobStatus.Description,
                    },
                });
                return Ok(loadsDTO);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }


        [HttpGet]
        [Route("GetLoadsCarried")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetLoadsCarried()
        {
            var results = await _reportRepository.GetLoadsCarriedAsync();
            var deliveries = await _jobRepository.GetAllDeliveries();
            var trucks = await _truckRepository.GetAllTrucksAsync();

            try
            {
                var truckDataList = new List<LoadsCarriedDTO>(); // Create a list to hold truck data

                foreach (var truck in trucks)
                {
                    double delweight = 0;
                    var trip = 0;

                    var registration = truck.Truck_License;
                    var truckid = truck.TruckID;
                    foreach (var del in deliveries)
                    {
                        if (del.TruckID == truckid)
                        {
                            delweight += del.Delivery_Weight;
                            trip++;
                        }
                    }

                    // Create a TruckData object and add it to the list
                    var truckData = new LoadsCarriedDTO
                    {
                        Registration = registration,
                        Weight = delweight,
                        Trip = trip
                    };

                    truckDataList.Add(truckData);
                }


                return Ok(truckDataList);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("GetAdmins")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        
        public async Task<IActionResult> GetAdmins()
        {
            var admins = await _adminRepository.GetAllAdminsAsync();
            

            try
            {
                var adminDataList = new List<AdminDTO>(); // Create a list to hold admin data

                foreach (var adm in admins)
                {


                    var admin_id = adm.Admin_ID;
                    var name = adm.Name;
                    var lastName = adm.Lastname;
                    var email = adm.Email;

                    // Create a AdminData object and add it to the list
                    var adminData = new AdminDTO
                    {
                        Admin_ID = admin_id,
                        Name = name,
                        Lastname = lastName,
                        Email = email,
                    };

                    adminDataList.Add(adminData);
                }


                return Ok(adminDataList);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }


        [HttpGet]
        [Route("GetDrivers")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]

        public async Task<IActionResult> GetDrivers()
        {
            var drivers = await _driverRepository.GetAllDriversAsync();


            try
            {
                var driverDataList = new List<DriverDTO>(); // Create a list to hold driver data

                foreach (var driver in drivers)
                {


                    var driverID = driver.Driver_ID;
                    var name = driver.Name;
                    var lastName = driver.Lastname;
                    var email = driver.Email;
                    var phoneNum = driver.PhoneNumber;
                    var driverStatusID = driver.Driver_Status_ID;
                    var driverStatus = driver.DriverStatus;

                    // Create a DriverData object and add it to the list
                    var driverData = new DriverDTO
                    {
                        Driver_ID = driverID,
                        Name = name,
                        Lastname = lastName,
                        Email = email,
                        PhoneNumber = phoneNum,
                        Driver_Status_ID = driverStatusID,
                        DriverStatus = driverStatus,
                    };

                    driverDataList.Add(driverData);
                }


                return Ok(driverDataList);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

            [HttpGet]
            [Route("GetJobDetails")]
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
            public async Task<IActionResult> GetJobDetails()
            {

            var jobs = await _jobRepository.GetAllAdminJobsAsync();
            var deliveries = await _jobRepository.GetAllDeliveries();
            
            //client jobs?


            try
                {
                    var jobDataList = new List<JobDetailDTO>(); // Create a list to hold jobDetail data

                    foreach (var job in jobs)
                    {
                        double jobWeight = 0;
                        var trip = 0;
                        

                        var jobID = job.Job_ID;
                        
                        foreach (var del in deliveries)
                        {
                            if (del.Job_ID == jobID)
                            {
                                jobWeight += del.Delivery_Weight;
                                trip++;
                            }
                        }

                        // Create a JobDetailData object and add it to the list
                        var jobDetailData = new JobDetailDTO
                        {
                            Job_ID = jobID,
                            Total_Weight= jobWeight,
                            Trips = trip
                        };

                        jobDataList.Add(jobDetailData);
                    }


                    return Ok(jobDataList);
                }
                catch (Exception)
                {
                    return StatusCode(500, "Internal Server Error. Please contact support.");
                }
            }
        }

    }

