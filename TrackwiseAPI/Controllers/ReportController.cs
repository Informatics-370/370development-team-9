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

        public ReportController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        [HttpGet]
        [Route("GetLoadsCarried")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetLoadsCarried()
        {
            try
            {
                var results = await _reportRepository.GetLoadsCarriedAsync();
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
    }
}
