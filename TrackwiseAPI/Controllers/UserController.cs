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
using System.Runtime.Intrinsics.X86;
using Microsoft.EntityFrameworkCore;
using System.Web;
using TrackwiseAPI.Models.Password;

namespace TrackwiseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<AppUser> _claimsPrincipalFactory;
        private readonly IConfiguration _configuration;
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmailService _emailService;
        private readonly TwDbContext _context;

        public UserController(UserManager<AppUser> userManager,
            IUserClaimsPrincipalFactory<AppUser> claimsPrincipalFactory,
            IConfiguration configuration,
            ICustomerRepository customerRepository, 
            TwDbContext context, 
            IEmailService emailService)
        {
            _userManager = userManager;
            _claimsPrincipalFactory = claimsPrincipalFactory;
            _configuration = configuration;
            _customerRepository = customerRepository;
            _context = context;
            _emailService = emailService;
        }


        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(CustomerVM cvm)
        {
            var customerId = Guid.NewGuid().ToString();
            var customer = new Customer { Customer_ID = customerId, Name = cvm.Name, LastName = cvm.LastName, Email = cvm.Email, Password = cvm.Password };

            try
            {
                _customerRepository.Add(customer);
                await _customerRepository.SaveChangesAsync();

                var user = new AppUser
                {
                    Id = customerId,
                    UserName = cvm.Email,
                    Email = cvm.Email
                };
                var result = await _userManager.CreateAsync(user, cvm.Password);

                await _userManager.AddToRoleAsync(user, "Customer");

                if (result.Errors.Count() > 0) 
                    return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error. Please contact support.");
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }

            return Ok(customer);

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
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    // To prevent user enumeration, always return Ok() even if the email doesn't exist in the database.
                    return Ok();
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var encodedToken = HttpUtility.UrlEncode(token);

                // Send the email using your email service
                await _emailService.SendPasswordResetEmailAsync(user.Email, encodedToken);

                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception or return an error response
                return StatusCode(500, "An error occurred while sending the password reset email.");
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPassword model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // To prevent user enumeration, always return Ok() even if the email doesn't exist in the database.
                return Ok();
            }

            var decodedToken = HttpUtility.UrlDecode(model.Token);
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, model.NewPassword);

            if (!result.Succeeded)
            {
                // Handle password reset failure
                return BadRequest("Failed to reset password.");
            }

            // You can clear the reset token and its expiration once the password is successfully reset.
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;
            await _userManager.UpdateAsync(user);

            return Ok("Password successfully reset.");
        }



        [HttpGet]
        private async Task<ActionResult> GenerateJWTToken(AppUser user)
        {
            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
        new Claim(ClaimTypes.Email, user.Email)
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
