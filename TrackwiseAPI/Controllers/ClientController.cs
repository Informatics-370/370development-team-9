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
    public class ClientController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;

        public ClientController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        //Get all clients
        [HttpGet]
        [Route("GetAllClients")]
        public async Task<IActionResult> GetAllClients()
        {
            try
            {
                var results = await _clientRepository.GetAllClientsAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        //Get a specific client
        [HttpGet]
        [Route("GetClient/{ClientID}")]
        public async Task<IActionResult> GetClientAsync(int ClientID)
        {
            try
            {
                var result = await _clientRepository.GetClientAsync(ClientID);

                if (result == null) return NotFound("Client does not exist");

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support");
            }
        }

        //Add a client
        [HttpPost]
        [Route("AddClient")]
        public async Task<IActionResult> AddClient(ClientVM cvm)
        {
            var client = new Client { Name = cvm.Name, LastName = cvm.LastName, PhoneNumber = cvm.PhoneNumber };

            try
            {
                _clientRepository.Add(client);
                await _clientRepository.SaveChangesAsync();

            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }

            return Ok(client);
        }

        //update client
        [HttpPut]
        [Route("EditClient/{ClientID}")]
        public async Task<ActionResult<ClientVM>> EditClient(int ClientID, ClientVM cvm)
        {
            try
            {
                var existingClient = await _clientRepository.GetClientAsync(ClientID);
                if (existingClient == null) return NotFound($"The client does not exist");

                existingClient.Name = cvm.Name;
                existingClient.LastName = cvm.LastName;
                existingClient.PhoneNumber = cvm.PhoneNumber;

                if (await _clientRepository.SaveChangesAsync())
                {
                    return Ok(existingClient);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("Your request is invalid.");
        }

        //Remove client
        [HttpDelete]
        [Route("DeleteClient/{ClientID}")]
        public async Task<IActionResult> DeleteClient(int ClientID)
        {
            try
            {
                var existingClient = await _clientRepository.GetClientAsync(ClientID);

                if (existingClient == null) return NotFound($"The client does not exist");
                _clientRepository.Delete(existingClient);

                if (await _clientRepository.SaveChangesAsync()) return Ok(existingClient);

            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("Your request is invalid.");
        }
    }
}

