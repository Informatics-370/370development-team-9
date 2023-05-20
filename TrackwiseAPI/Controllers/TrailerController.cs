using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrackwiseAPI.Models.Entities;
using TrackwiseAPI.Models.Interfaces;
using TrackwiseAPI.Models.Repositories;
using TrackwiseAPI.Models.ViewModels;

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

        //Get all trailers
        [HttpGet]
        [Route("GetAllTrailers")]
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

        //Get a specific trailer
        [HttpGet]
        [Route("GetTrailer/{TrailerID}")]
        public async Task<IActionResult> GetTrailerAsync(int TrailerID)
        {
            try
            {
                var result = await _trailerRepository.GetTrailerAsync(TrailerID);

                if (result == null) return NotFound("Trailer does not exist");

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support");
            }
        }

        //Add a trailer
        [HttpPost]
        [Route("AddTrailer")]
        public async Task<IActionResult> AddTrailer(TrailerVM tvm)
        {
            var trailer = new Trailer { Trailer_License = tvm.Trailer_License, Model = tvm.Model, Weight = tvm.Weight, Trailer_Status_ID = tvm.Trailer_Status_ID, Trailer_Type_ID = tvm.Trailer_Type_ID };

            try
            {
                _trailerRepository.Add(trailer);
                await _trailerRepository.SaveChangesAsync();

            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }

            return Ok(trailer);
        }

        //update trailer
        [HttpPut]
        [Route("EditTrailer/{TrailerID}")]
        public async Task<ActionResult<TrailerVM>> EditTrailer(int TrailerID, TrailerVM tvm)
        {
            try
            {
                var existingTrailer = await _trailerRepository.GetTrailerAsync(TrailerID);
                if (existingTrailer == null) return NotFound($"The trailer does not exist");

                existingTrailer.Trailer_License = tvm.Trailer_License;
                existingTrailer.Model = tvm.Model;
                existingTrailer.Weight = tvm.Weight;
                existingTrailer.Trailer_Status_ID = tvm.Trailer_Status_ID;
                existingTrailer.Trailer_Type_ID = tvm.Trailer_Type_ID;
                if (await _trailerRepository.SaveChangesAsync())
                {
                    return Ok(existingTrailer);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("Your request is invalid.");
        }

        //Remove trailer
        [HttpDelete]
        [Route("DeleteTrailer/{TrailerID}")]
        public async Task<IActionResult> DeleteTrailer(int TrailerID)
        {
            try
            {
                var existingTrailer = await _trailerRepository.GetTrailerAsync(TrailerID);

                if (existingTrailer == null) return NotFound($"The trailer does not exist");
                _trailerRepository.Delete(existingTrailer);

                if (await _trailerRepository.SaveChangesAsync()) return Ok(existingTrailer);

            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("Your request is invalid.");
        }
    }
}
