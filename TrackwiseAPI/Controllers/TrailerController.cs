﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using TrackwiseAPI.Models.Entities;
using TrackwiseAPI.Models.Interfaces;
using TrackwiseAPI.Models.Repositories;
using TrackwiseAPI.Models.ViewModels;

namespace TrackwiseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class TrailerController : ControllerBase
    {
        private readonly ITrailerRepository _trailerRepository;
        private readonly IAuditRepository _auditRepository;

        public TrailerController(ITrailerRepository trailerRepository, IAuditRepository auditRepository)
        {
            _trailerRepository = trailerRepository;
            _auditRepository = auditRepository;
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
        public async Task<IActionResult> GetTrailerAsync(string TrailerID)
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
            var trailerId = Guid.NewGuid().ToString();
            var trailer = new Trailer { TrailerID = trailerId, Trailer_License = tvm.Trailer_License, Model = tvm.Model, Weight = tvm.Weight, Trailer_Status_ID = tvm.Trailer_Status_ID, Trailer_Type_ID = tvm.Trailer_Type_ID };
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var auditId = Guid.NewGuid().ToString();
            var audit = new Audit { Audit_ID = auditId, Action = "Add Trailer", CreatedDate = DateTime.Now, User = userEmail };
            try
            {
                _trailerRepository.Add(trailer);
                await _trailerRepository.SaveChangesAsync();
                _auditRepository.Add(audit);
                await _auditRepository.SaveChangesAsync();
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
        public async Task<ActionResult<TrailerVM>> EditTrailer(string TrailerID, TrailerVM tvm)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var auditId = Guid.NewGuid().ToString();
            var audit = new Audit { Audit_ID = auditId, Action = "Update Trailer", CreatedDate = DateTime.Now, User = userEmail };
            try
            {
                var existingTrailer = await _trailerRepository.GetTrailerAsync(TrailerID);
                if (existingTrailer == null) return NotFound($"The trailer does not exist");

                if (existingTrailer.Trailer_License == tvm.Trailer_License &&
                    existingTrailer.Model == tvm.Model &&
                    existingTrailer.Weight == tvm.Weight &&
                    existingTrailer.Trailer_Status_ID == tvm.Trailer_Status_ID &&
                    existingTrailer.Trailer_Type_ID == tvm.Trailer_Type_ID)
                {
                    // No changes made, return the existing driver without updating
                    return Ok(existingTrailer);
                }

                existingTrailer.Trailer_License = tvm.Trailer_License;
                existingTrailer.Model = tvm.Model;
                existingTrailer.Weight = tvm.Weight;
                existingTrailer.Trailer_Status_ID = tvm.Trailer_Status_ID;
                existingTrailer.Trailer_Type_ID = tvm.Trailer_Type_ID;
                if (await _trailerRepository.SaveChangesAsync())
                {
                    _auditRepository.Add(audit);
                    await _auditRepository.SaveChangesAsync();
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
        public async Task<IActionResult> DeleteTrailer(string TrailerID)
        {
            try
            {
                var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var auditId = Guid.NewGuid().ToString();
                var audit = new Audit { Audit_ID = auditId, Action = "Delete Trailer", CreatedDate = DateTime.Now, User = userEmail };
                var existingTrailer = await _trailerRepository.GetTrailerAsync(TrailerID);

                if (existingTrailer == null) return NotFound($"The trailer does not exist");
                _trailerRepository.Delete(existingTrailer);

                if (await _trailerRepository.SaveChangesAsync())
                {
                    _auditRepository.Add(audit);
                    await _auditRepository.SaveChangesAsync();
                    return Ok(existingTrailer);
                } 
                   

            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("Your request is invalid.");
        }
    }
}
