using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrackwiseAPI.Models.Interfaces;

namespace TrackwiseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrailerController : ControllerBase
    {
        private readonly ITrailerRepository _trailerRepository;

        public TrailerController(ITrailerRepository trailerRepository)
        {
            _trailerRepository = trailerRepository;
        }

        //get trailers
        [HttpGet]
        [Route("GetAllTrailer")]
        public async Task<IActionResult> GetAllTrailers()
        {
            try
            {
                var results = await _trailerRepository.GetAllTrailerAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }
    }
}
