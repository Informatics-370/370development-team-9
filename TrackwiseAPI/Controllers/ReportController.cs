using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using TrackwiseAPI.DBContext;
using TrackwiseAPI.Models.DataTransferObjects;
using TrackwiseAPI.Models.Email;
using TrackwiseAPI.Models.Entities;
using TrackwiseAPI.Models.Interfaces;
using TrackwiseAPI.Models.Repositories;
using TrackwiseAPI.Models.ViewModels;

namespace TrackwiseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportRepository _reportRepository;
        private readonly ITruckRepository _truckRepository;
        private readonly IJobRepository _jobRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IDriverRepository _driverRepository;
        private readonly IAdminRepository _adminRepository;



        public ReportController(
            IReportRepository reportRepository, 
            ITruckRepository truckRepository,
            IJobRepository jobRepository,
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IDriverRepository driverRepository,
            IAdminRepository adminRepository
            )

        {
            _reportRepository = reportRepository;
            _truckRepository = truckRepository;
            _jobRepository = jobRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _driverRepository = driverRepository;
            _adminRepository = adminRepository;
        }

        [HttpGet]
        [Route("GetJobDetail")]
        public async Task<IActionResult> GetJobDetail()
        {
            var deliveries = await _jobRepository.GetAllDeliveries();
            var jobs = await _jobRepository.GetAllAdminJobsAsync();
            try
            {
                var joblist = new List<JobDetailDTO>(); // Create a list to hold job data
                foreach (var job in jobs)
                {
                    var jobData = new JobDetailDTO
                    {
                        Job_ID = job.Job_ID,
                        Pickup_Location = job.Pickup_Location,
                        Dropoff_Location = job.Dropoff_Location,
                        StartDate = job.StartDate,
                        DueDate = job.DueDate,
                        Total_Weight = 0, // Initialize total weight to 0
                        Total_Trips = 0, // Initialize total trips to 0
                        deliveryList = new List<deliveryDetailDTO>() // Create a list for delivery details
                    };
                    foreach (var del in deliveries)
                    {
                        if (del.Job_ID == job.Job_ID)
                        {
                            jobData.Total_Weight += del.Delivery_Weight; // Add delivery weight to total weight
                            jobData.Total_Trips++; // Increment total trips
                            var deliveryDetailDTO = new deliveryDetailDTO
                            {
                                Delivery_ID = del.Delivery_ID,
                                Delivery_Weight = del.Delivery_Weight,
                                Trips = jobData.Total_Trips, // Use total trips for this delivery
                            };
                            jobData.deliveryList.Add(deliveryDetailDTO);
                        }
                    }
                    joblist.Add(jobData); // Add job data to the list
                }
                return Ok(joblist);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("GetDrivers")]
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

                    // Create a DriverData object and add it to the list
                    var driverData = new DriverDTO
                    {
                        Name = name,
                        Lastname = lastName,
                        Email = email,
                        PhoneNumber = phoneNum,
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
        [Route("GetAdmins")]

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
        [Route("GetCompleteJobs/{truckID}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetCompleteJobs(string truckID)
        {
            try
            {
                var results = await _reportRepository.GetCompleteJobsAsync();

                // Filter the jobs where delivery.TruckID matches the provided truckID
                var matchingJobs = results.Where(job => job.Deliveries.Any(delivery => delivery.TruckID == truckID)).ToList();

                return Ok(matchingJobs);
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



        //[HttpGet]
        //[Route("GetOrders")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        //public  async Task<IActionResult> GetOrders()
        //{
        //    var result = await _reportRepository.GetOrdersAsync();
        //    var product = await _productRepository.GetAllProductsAsync();
        //    var prodType = await _productRepository.GetProductTypeAsync();
        //    var prodCategory = await _productRepository.GetProductCategoryAsync();


        //    try
        //    {
        //        var salesListData = new List<SalesDTO>(); // Create a list to hold truck data

        //        foreach (var prod in product) 
        //        {


        //            return Ok(salesListData);
        //        }
        //    }
        //    catch (Exception)
        //    {


        //[HttpGet]
        //[Route("GetAllMileageFuel")]
        //public async Task<IActionResult> GetAllMileageFuel()
        //{
        //    //var results = await _reportRepository.GetLoadsCarriedAsync();
        //    var deliveries = await _jobRepository.GetAllDeliveries();
        //    var trucks = await _truckRepository.GetAllTrucksAsync();

        //    try
        //    {
        //        var truckDataList = new List<MileageFuelDTO>(); // Create a list to hold truck data

        //        foreach (var truck in trucks)
        //        {
        //            double? mileage = 0;
        //            double? fuel = 0;


        //            var registration = truck.Truck_License;
        //            var truckid = truck.TruckID;
        //            foreach (var del in deliveries)
        //            {
        //                if (del.TruckID == truckid)
        //                {
        //                    mileage = del.Final_Mileage - del.Initial_Mileage;
        //                    fuel = del.TotalFuel;
        //                }
        //            }

        //            // Create a TruckData object and add it to the list
        //            var truckData = new MileageFuelDTO
        //            {
        //                Registration = registration,
        //                Total_Mileage = mileage,
        //                Total_Fuel = fuel,
        //            };

        //            truckDataList.Add(truckData);
        //        }


        //        return Ok(truckDataList);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500, "Internal Server Error. Please contact support.");
        //    }
        //}#

        [HttpGet]
        [Route("GetAllMileageFuel")]
        public async Task<IActionResult> GetAllMileageFuel()
        {
            var deliveries = await _jobRepository.GetAllDeliveries();
            var trucks = await _truckRepository.GetAllTrucksAsync();

            try
            {
                var truckDataList = new List<TruckDataDTO>(); // Create a list to hold truck data

                foreach (var truck in trucks)
                {
                    var truckData = new TruckDataDTO
                    {
                        Registration = truck.Truck_License,
                        MFList = new List<MileageFuelDTO>() // Create a list for MileageFuelDTO objects
                    };

                    foreach (var del in deliveries)
                    {
                        if (del.TruckID == truck.TruckID)
                        {
                            var mileage = del.Final_Mileage - del.Initial_Mileage;
                            var fuel = del.TotalFuel;

                            var mileageFuelData = new MileageFuelDTO
                            {
                                Delivery_ID = del.Delivery_ID,
                                Mileage = mileage,
                                Fuel = fuel
                            };

                            truckData.MFList.Add(mileageFuelData);
                        }
                    }

                    truckDataList.Add(truckData); // Move this line here
                }

                return Ok(truckDataList);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("GetTotalSales")]
        public async Task<IActionResult> GetTotalSales()
        {
            var sales = await _orderRepository.GetAllOrdersAsync();
            var totalSalesPerMonth = new Dictionary<string, TotalSalesDTO>();
            try
            {
                var total = 0.0;
                var amount = 0;

                foreach (var order in sales)
                {
                    var orderMonth = order.Date.ToString("yyyy-MM");
                    if (!totalSalesPerMonth.ContainsKey(orderMonth))
                    {
                        totalSalesPerMonth[orderMonth] = new TotalSalesDTO
                        {
                            Total = 0.0,
                            Amount = 0,
                            Date = order.Date
                        };
                    }
                    totalSalesPerMonth[orderMonth].Total += order.Total;
                    totalSalesPerMonth[orderMonth].Amount++;

                };

                var result = totalSalesPerMonth.Values.ToList();

                return Ok(result);
            } 
            catch (Exception) { return StatusCode(500, "Internal Server Error."); }
        }

        [HttpGet]
        [Route("GetJobListing")]
        public async Task<IActionResult> GetJobListing()
        {
            //ek soek admin jobs
            var jobs = await _jobRepository.GetAllAdminJobsAsync();
            var deliveries = await _jobRepository.GetAllDeliveries();
            try
            {
                var JoblistData = new List<JobListingDTO>(); // Create a list to hold truck data

                foreach (var job in jobs)
                {
                    double delweight = 0;
                    var trip = 0;
                    foreach (var del in deliveries)
                    {
                        if (del.Job_ID == job.Job_ID)
                        {
                            delweight += del.Delivery_Weight;
                            trip++;
                        }

                    }
                    var Data = new JobListingDTO
                    {
                        Job_ID = job.Job_ID,
                        Weight = delweight,
                        Trips = trip,
                        Creator = job.Creator_ID
                    };
                    JoblistData.Add(Data); // Move this line here
                }

                return Ok(JoblistData);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            
        }

    }
}
