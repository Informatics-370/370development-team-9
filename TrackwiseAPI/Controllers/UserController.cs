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
using Azure;
using Newtonsoft.Json.Linq;
using System.Data;

namespace TrackwiseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUserClaimsPrincipalFactory<AppUser> _claimsPrincipalFactory;
        private readonly IConfiguration _configuration;
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmailService _emailService;
        private readonly TwDbContext _context;
        private readonly MailController _mailController;
        private readonly IAuditRepository _auditRepository;

        public UserController(UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager,
            IUserClaimsPrincipalFactory<AppUser> claimsPrincipalFactory,
            IConfiguration configuration,
            ICustomerRepository customerRepository, 
            TwDbContext context, 
            IEmailService emailService,
            MailController mailController, IAuditRepository auditRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
            try
            {
                _customerRepository.Add(customer);
                await _customerRepository.SaveChangesAsync();
                //_auditRepository.Add(audit);
                await _auditRepository.SaveChangesAsync();

                var user = new AppUser
                {
                    Id = customerId,
                    UserName = cvm.Email,
                    Email = cvm.Email
                };
                var result = await _userManager.CreateAsync(user, cvm.Password);

                await _userManager.AddToRoleAsync(user, "Customer");

                var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var auditId = Guid.NewGuid().ToString();
                var audit = new Audit { Audit_ID = auditId, Action = "Register Customer", CreatedDate = DateTime.Now, User = userEmail };
                _auditRepository.Add(audit);

                //Add Token to Verify the email....
                var confirmationLink = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedToken = confirmationLink.Replace("/", "%2F");

                var confirmationMail = new ConfirmEmail
                {
                    Email = user.Email,
                    Name = user.UserName,

                    ConfirmationLink = "http://localhost:4200/Authentication/confirm-email/" + encodedToken + "/" + user.UserName
                };

                var mail = await _mailController.ConfirmEmail(confirmationMail);

                /*var confirmationLink = Url.Action(nameof(ConfirmEmail), "Authentication", new { token, email = user.Email }, Request.Scheme);
                var message = new Message(new string[] { user.Email! }, "Confirmation email link", confirmationLink!);
                _emailService.SendEmail(message);*/

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
        [Route("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailVM confirmEmailVM)
        {
            var user = await _userManager.FindByEmailAsync(confirmEmailVM.Email);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, confirmEmailVM.Token);
                if (result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status200OK);
                }
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }


            [HttpPost]
            [Route("Login")]
            public async Task<ActionResult> Login(UserVM uvm)
            {
                var user = await _userManager.FindByNameAsync(uvm.emailaddress);
            if(user == null)
            {
                return NotFound("User does not exist or invalid credentials");
            }

            if (user.EmailConfirmed == false)
            {
                //Add Token to Verify the email....
                var confirmationLink = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedToken = confirmationLink.Replace("/", "%2F");

                var confirmationMail = new ConfirmEmail
                {
                    Email = user.Email,
                    Name = user.UserName,

                    ConfirmationLink = "http://localhost:4200/Authentication/confirm-email/" + encodedToken + "/" + user.UserName
                };

                var roles = await _userManager.GetRolesAsync(user); // Get the roles associated with the user

                var response = new
                {
                    Token = "",
                    Role = roles.FirstOrDefault(),
                    isTwoFactor = user.TwoFactorEnabled,
                    isEmailConfirmed = user.EmailConfirmed
                };

                await _mailController.ConfirmEmail(confirmationMail);

                return Ok(response);
            }

            if (user.TwoFactorEnabled)
                {
                    await _signInManager.SignOutAsync();
                    await _signInManager.PasswordSignInAsync(user, uvm.password, false, true);

                var roles = await _userManager.GetRolesAsync(user);

                await GenerateOTPFor2StepVerification(user);

                // Calculate token expiration time
                var tokenCreationTime = DateTime.UtcNow;
                var tokenExpirationTime = tokenCreationTime.AddMinutes(5);

                var response = new
                {
                    Token = new { value = new { token = "", user = user.UserName } },
                    Role = roles.FirstOrDefault(),
                    isTwoFactor = user.TwoFactorEnabled,
                    isEmailConfirmed = user.EmailConfirmed,
                    expireOTPtime = tokenExpirationTime
                };

                return Ok(response);
                }

                if (user != null && await _userManager.CheckPasswordAsync(user, uvm.password))
                {
                    try
                    {
                    var auditId = Guid.NewGuid().ToString();

                    var audit = new Audit { Audit_ID = auditId, Action = "Login", CreatedDate = DateTime.Now, User = user.Email };

                    var token = await GenerateJWTToken(user); // Generate the JWT token

                        var roles = await _userManager.GetRolesAsync(user); // Get the roles associated with the user

                        var response = new
                        {
                            Token = token,
                            Role = roles.FirstOrDefault(),
                            isTwoFactor = user.TwoFactorEnabled,
                            isEmailConfirmed = user.EmailConfirmed
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

        private async Task<IActionResult> GenerateOTPFor2StepVerification(AppUser user)
        {
            var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);
            if (!providers.Contains("Email"))
            {
                return Unauthorized();
            }

            var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
            var twoFactorMail = new TwoFactor
            {
                Email = user.Email,
                Name = user.UserName,
                twoFactorOTP = token
            };

            var roles = await _userManager.GetRolesAsync(user);

            var response = new
            {
                Token = new { value = new { token = "", user = user.UserName } },
                Role = roles.FirstOrDefault()
            };

            await _mailController.TwoFactorEmail(twoFactorMail);

            return Ok();
        }

        [HttpPost("TwoStepVerification")]
        public async Task<IActionResult> TwoStepVerification(TwoFactorVM twoFactorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userManager.FindByEmailAsync(twoFactorDto.Username);
            if (user is null)
                return BadRequest("Invalid Request");

            var validVerification = await _userManager.VerifyTwoFactorTokenAsync(user, "Email", twoFactorDto.Code);
            if (!validVerification)
                return BadRequest("Invalid Token Verification");

            var auditId = Guid.NewGuid().ToString();
            var audit = new Audit { Audit_ID = auditId, Action = "Login", CreatedDate = DateTime.Now, User = user.Email };
            _auditRepository.Add(audit);
            await _auditRepository.SaveChangesAsync();

            var token = await GenerateJWTToken(user);

            var roles = await _userManager.GetRolesAsync(user);

            var response = new
            {
                Token = token,
                Role = roles.FirstOrDefault()
            };

            return Ok(response);
        }

        [HttpPost]
        [Route("ResendTwoFactor/{username}")]
        public async Task<IActionResult> ResendTwoFactor(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound("User does not exist or invalid credentials");
            }


            if (user.TwoFactorEnabled)
            {

                var roles = await _userManager.GetRolesAsync(user);

                await GenerateOTPFor2StepVerification(user);

                // Calculate token expiration time
                var tokenCreationTime = DateTime.UtcNow;
                var tokenExpirationTime = tokenCreationTime.AddMinutes(5);

                var response = new
                {
                    Token = new { value = new { token = "", user = user.UserName } },
                    Role = roles.FirstOrDefault(),
                    isTwoFactor = user.TwoFactorEnabled,
                    isEmailConfirmed = user.EmailConfirmed,
                    expireOTPtime = tokenExpirationTime
                };

                return Ok(response);
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
