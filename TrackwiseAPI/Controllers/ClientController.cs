using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using TrackwiseAPI.DBContext;
using TrackwiseAPI.Models.Email;
using TrackwiseAPI.Models.Entities;
using TrackwiseAPI.Models.Interfaces;
using TrackwiseAPI.Models.Password;
using TrackwiseAPI.Models.Repositories;
using TrackwiseAPI.Models.ViewModels;

namespace TrackwiseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<AppUser> _claimsPrincipalFactory;
        private readonly IConfiguration _configuration;
        private readonly IClientRepository _clientRepository;
        private readonly MailController _mailController;
        private readonly IAuditRepository _auditRepository;

        public ClientController(
            UserManager<AppUser> userManager,
            IUserClaimsPrincipalFactory<AppUser> claimsPrincipalFactory,
            IConfiguration configuration,
            IClientRepository clientRepository,
            MailController mailController, IAuditRepository auditRepository
            )
        {
            _userManager = userManager;
            _claimsPrincipalFactory = claimsPrincipalFactory;
            _configuration = configuration;
            _clientRepository = clientRepository;
            _mailController = mailController;
            _auditRepository = auditRepository;
        }
    
        //Get all clients
        [HttpGet]
        [Route("GetAllClients")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Client")]
        public async Task<IActionResult> GetClientAsync(string ClientID)
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> AddClient(ClientVM cvm)
        {
            var clientId = Guid.NewGuid().ToString();
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var auditId = Guid.NewGuid().ToString();
            var audit = new Audit { Audit_ID = auditId, Action = "Add Client", CreatedDate = DateTime.Now, User = userEmail };

            var client = new Client { Client_ID = clientId, Name = cvm.Name, PhoneNumber = cvm.PhoneNumber, Email = cvm.Email };
            var newclientmail = new NewClientMail { Email = client.Email , Name = client.Name, PhoneNumber = client.PhoneNumber, Password = cvm.Password };
            var existingadmin = await _userManager.FindByNameAsync(cvm.Email);
            if (existingadmin != null) return BadRequest("User already exists");
            try
            {
                _clientRepository.Add(client);
                await _clientRepository.SaveChangesAsync();
                _auditRepository.Add(audit);
                await _auditRepository.SaveChangesAsync();
                var user = new AppUser
                {
                    Id = clientId,
                    UserName = cvm.Email,
                    Email = cvm.Email
                };

                var result = await _userManager.CreateAsync(user, cvm.Password);
                var mail = await _mailController.SendClientEmail(newclientmail);


                await _userManager.AddToRoleAsync(user, "Client");

                if (result.Errors.Count() > 0)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error. Please contact support.");

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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Client")]
        public async Task<ActionResult<ClientVM>> EditClient(string ClientID, ClientVM cvm)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var auditId = Guid.NewGuid().ToString();
            var audit = new Audit { Audit_ID = auditId, Action = "Update Client", CreatedDate = DateTime.Now, User = userEmail };
            try
            {
                var existingClient = await _clientRepository.GetClientAsync(ClientID);
                if (existingClient == null) 
                    return NotFound($"The client does not exist");

                var existingUser = await _userManager.FindByIdAsync(ClientID);
                if (existingUser == null)
                    return NotFound($"The corresponding user does not exist");

                if (existingClient.Name == cvm.Name &&
                    existingClient.PhoneNumber == cvm.PhoneNumber &&
                    existingClient.Email == cvm.Email)
                {
                    // No changes made, return the existing driver without updating
                    return Ok(existingClient);
                }

                existingClient.Name = cvm.Name;
                existingClient.PhoneNumber = cvm.PhoneNumber;
                existingClient.Email = cvm.Email;

                existingUser.UserName = cvm.Email;
                existingUser.Email = cvm.Email;
                await _userManager.RemovePasswordAsync(existingUser);
                await _userManager.AddPasswordAsync(existingUser, cvm.Password);
                existingUser.SecurityStamp = Guid.NewGuid().ToString();

                var clientUpdateResult = await _clientRepository.SaveChangesAsync();
                var userUpdateResult = await _userManager.UpdateAsync(existingUser);

                if (clientUpdateResult && userUpdateResult.Succeeded)
                {
                    _auditRepository.Add(audit);
                    await _auditRepository.SaveChangesAsync();
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> DeleteClient(string ClientID)
        {
            try
            {
                var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var auditId = Guid.NewGuid().ToString();
                var audit = new Audit { Audit_ID = auditId, Action = "Delete Client", CreatedDate = DateTime.Now, User = userEmail };
                var existingClient = await _clientRepository.GetClientAsync(ClientID);

                if (existingClient == null) 
                    return NotFound($"The client does not exist");

                var user = await _userManager.FindByEmailAsync(existingClient.Email);
                if (user != null)
                {
                    var result = await _userManager.DeleteAsync(user);
                    if (!result.Succeeded)
                    {
                        return StatusCode(500, "Failed to delete the associated user.");
                    }
                }

                _clientRepository.Delete(existingClient);

                if (await _clientRepository.SaveChangesAsync())
                {
                    _auditRepository.Add(audit);
                    await _auditRepository.SaveChangesAsync();
                    return Ok(existingClient);
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

