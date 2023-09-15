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
using System.Security.Cryptography;
using TrackwiseAPI.Models.Email;

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
        private readonly MailController _mailController;
        private readonly IAuditRepository _auditRepository;

        public UserController(UserManager<AppUser> userManager,
            IUserClaimsPrincipalFactory<AppUser> claimsPrincipalFactory,
            IConfiguration configuration,
            ICustomerRepository customerRepository, 
            TwDbContext context, 
            IEmailService emailService,
            MailController mailController, IAuditRepository auditRepository)
        {
            _userManager = userManager;
            _claimsPrincipalFactory = claimsPrincipalFactory;
            _configuration = configuration;
            _customerRepository = customerRepository;
            _context = context;
            _emailService = emailService;
            _mailController = mailController;
            _auditRepository = auditRepository;
        }


        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(CustomerVM cvm)
        {
            var customerId = Guid.NewGuid().ToString();
            var customer = new Customer { Customer_ID = customerId, Name = cvm.Name, LastName = cvm.LastName, Email = cvm.Email };
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var auditId = Guid.NewGuid().ToString();
            var audit = new Audit { Audit_ID = auditId, Action = "Register Customer", CreatedDate = DateTime.Now, User = userEmail };
            try
            {
                _customerRepository.Add(customer);
                await _customerRepository.SaveChangesAsync();
                _auditRepository.Add(audit);
                await _auditRepository.SaveChangesAsync();

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
            var auditId = Guid.NewGuid().ToString();
            var audit = new Audit { Audit_ID = auditId, Action = "Login", CreatedDate = DateTime.Now, User = user.Email };

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
                    _auditRepository.Add(audit);
                    await _auditRepository.SaveChangesAsync();
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
        /*
        [HttpPost("forgot-password/{email}")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            var newclientmail = new NewClientMail { Email = user.Email, Name =user.UserName, Password="Test12345",PhoneNumber=user.PhoneNumber };

            if (user==null)
            {
                return BadRequest("User not found");
            }

            //var mail = await _mailController.ForgotPasswordEmail(newclientmail);
            await _context.SaveChangesAsync();
            return Ok("Email has been sent");
        }*/

        /*
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPassword request)
        {
            //var user1 = await _userManager.FindByEmailAsync(request.Email);
            //var user = await _context.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == request.Token);
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null || user.ResetTokenExpires < DateTime.Now)
            {
                return BadRequest("Invalid token");
            }

            var resetPasswordResult = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
            if (!resetPasswordResult.Succeeded)
            {
                // Password reset failed. You may handle specific error cases here if needed.
                return BadRequest("Password reset failed.");
            }
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;
            await _context.SaveChangesAsync();

            return Ok("Password successfully reset.");
        }

        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
        */

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
