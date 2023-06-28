using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TrackwiseAPI.DBContext;
using TrackwiseAPI.Models.Interfaces;
using TrackwiseAPI.Models.Repositories;
using TrackwiseAPI.Models.ViewModels;
using TrackwiseAPI.Models.Entities;
using Microsoft.AspNetCore.Authorization;

namespace TrackwiseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<AppUser> _claimsPrincipalFactory;
        private readonly IConfiguration _configuration;
        private readonly IAdminRepository _adminRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IDriverRepository _driverRepository;
        private readonly IProductRepository _productRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly ITrailerRepository _trailerRepository;
        private readonly ITruckRepository _truckRepository;

        public UserController(UserManager<AppUser> userManager,
     IUserClaimsPrincipalFactory<AppUser> claimsPrincipalFactory,
            IConfiguration configuration,
            IAdminRepository adminRepository,
            IClientRepository clientRepository,
            ICustomerRepository customerRepository,
            IDriverRepository driverRepository,
            IProductRepository productRepository,
            ISupplierRepository supplierRepository,
            ITrailerRepository trailerRepository,
            ITruckRepository truckRepository)
        {
            _userManager = userManager;
            _claimsPrincipalFactory = claimsPrincipalFactory;
            _configuration = configuration;
            _adminRepository = adminRepository;
            _clientRepository = clientRepository;
            _customerRepository = customerRepository;
            _driverRepository = driverRepository;
            _productRepository = productRepository;
            _supplierRepository = supplierRepository;
            _trailerRepository = trailerRepository;
            _truckRepository = truckRepository;
        }
/*
        [HttpPost]
        [Route("AddNewAdmin")]
        public async Task<IActionResult> AddNewAdmin(AdminVM avm)
        {
            var adminId = Guid.NewGuid().ToString();

            var admin = new Admin { Admin_ID = adminId, Name = avm.Name, Lastname = avm.Lastname, Email = avm.Email, Password = avm.Password };

            try
            {
                _adminRepository.Add(admin);
                await _adminRepository.SaveChangesAsync();

                var user = new AppUser
                {
                    Id = adminId,
                    UserName = avm.Email,
                    Email = avm.Email
                };

                var result = await _userManager.CreateAsync(user, avm.Password);

                await _userManager.AddToRoleAsync(user, "Admin");

                if (result.Errors.Count() > 0)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error. Please contact support.");

            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }

            return Ok(admin);
        }
*/


        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(UserVM uvm)
        {
            var user = await _userManager.FindByIdAsync(uvm.emailaddress);

            if (user == null)
            {
                user = new AppUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = uvm.emailaddress,
                    Email = uvm.emailaddress
                };

                var result = await _userManager.CreateAsync(user, uvm.password);

                await _userManager.AddToRoleAsync(user, "Customer");

                if (result.Errors.Count() > 0) return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error. Please contact support.");
            }
            else
            {
                return Forbid("Account already exists.");
            }

            return Ok();
        }


        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login(UserVM uvm)
        {
            var user = await _userManager.FindByNameAsync(uvm.emailaddress);

            if (user != null && await _userManager.CheckPasswordAsync(user, uvm.password))
            {
                try
                {
                    var token = await GenerateJWTToken(user); // Generate the JWT token

                    var roles = await _userManager.GetRolesAsync(user); // Get the roles associated with the user

                    var response = new
                    {
                        Token = token,
                        Role = roles.FirstOrDefault()
                    };
                    return Ok(response);
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error. Please contact support.");
                }
            }
            else
            {
                return NotFound("User does not exist or invalid credentials");
            }
        }



        [HttpGet]
        private async Task<ActionResult> GenerateJWTToken(AppUser user)
        {
            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
    };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Tokens:Issuer"],
                _configuration["Tokens:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { token = tokenString, user = user.UserName });
        }
    }
   }
