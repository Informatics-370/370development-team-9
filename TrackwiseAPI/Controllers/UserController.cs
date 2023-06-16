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
                    //var principal = await _claimsPrincipalFactory.CreateAsync(user);
                    //await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal);
                    return GenerateJWTToken(user);
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error. Please contact support.");
                }
            }
            else
            {
                return NotFound("Does not exist");
            }

            //var loggedInUser = new UserViewModel { EmailAddress = uvm.EmailAddress, Password = uvm.Password };

            //return Ok(loggedInUser);
        }

        [HttpGet]
        private ActionResult GenerateJWTToken(AppUser user)
        {
            // Create JWT Token
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Tokens:Issuer"],
                _configuration["Tokens:Audience"],
                claims,
                signingCredentials: credentials,
                expires: DateTime.UtcNow.AddHours(3)
            );

            return Created("", new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                user = user.UserName
            });
        }


    }
}
