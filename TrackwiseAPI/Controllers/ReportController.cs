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

        public ReportController(
            IReportRepository reportRepository, 
            ITruckRepository truckRepository,
            IJobRepository jobRepository,
            IOrderRepository orderRepository)
        {
            _reportRepository = reportRepository;
            _truckRepository = truckRepository;
            _jobRepository = jobRepository;
            _orderRepository = orderRepository;
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

    }
}
