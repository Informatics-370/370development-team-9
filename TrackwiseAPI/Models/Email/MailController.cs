using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrackwiseAPI.Models.Repositories;
using System.Collections.Generic;
using TrackwiseAPI.DBContext;
using Microsoft.AspNetCore.Identity;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace TrackwiseAPI.Models.Email
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mail;
        private readonly TwDbContext _context;
        private readonly UserManager<AppUser> _userManager;


        public MailController(IMailService mail, TwDbContext context, UserManager<AppUser> userManager)
        {
            _mail = mail;
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("sendmail")]
        public async Task<IActionResult> SendMailAsync(MailData mailData)
        {
            bool result = await _mail.SendAsync(mailData, new CancellationToken());

            if (result)
            {
                return StatusCode(StatusCodes.Status200OK, "Mail has successfully been sent.");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured. The Mail could not be sent.");
            }
        }


        [HttpPost("ClientCredentials")]
        public async Task<IActionResult> SendClientEmail(NewClientMail welcomeMail)
        {
            // Create MailData object
            MailData mailData = new MailData(
                new List<string> { welcomeMail.Email },
                "Login Credentials",
                _mail.GetEmailTemplate("newClient", welcomeMail));


            bool sendResult = await _mail.SendAsync(mailData, new CancellationToken());

            if (sendResult)
            {
                return StatusCode(StatusCodes.Status200OK, "Mail has successfully been sent using template.");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured. The Mail could not be sent.");
            }
        }

        [HttpPost("DriverCredentials")]
        public async Task<IActionResult> SendDriverEmail(NewDriverMail welcomeMail)
        {
            // Create MailData object
            MailData mailData = new MailData(
                new List<string> { welcomeMail.Email },
                "Login Credentials",
                _mail.GetEmailTemplate("newDriver", welcomeMail));


            bool sendResult = await _mail.SendAsync(mailData, new CancellationToken());

            if (sendResult)
            {
                return StatusCode(StatusCodes.Status200OK, "Mail has successfully been sent using template.");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured. The Mail could not be sent.");
            }
        }

        [HttpPost]
        [Route("Invoice")]
        public async Task<IActionResult> SendInvoiceEmail(newInvoice invoice)
        {
            // Create MailData object
            MailData mailData = new MailData(
                new List<string> { invoice.Email },
                "Order Invoice", 
                _mail.GetEmailTemplate("Invoice", invoice));


            bool sendResult = await _mail.SendAsync(mailData, new CancellationToken());

            if (sendResult)
            {
                return StatusCode(StatusCodes.Status200OK, "Mail has successfully been sent using template.");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured. The Mail could not be sent.");
            }
        }

        [HttpPost("AdminCredentials")]
        public async Task<IActionResult> SendAdminEmail(NewAdminMail welcomeMail)
        {
            // Create MailData object
            MailData mailData = new MailData(
                new List<string> { welcomeMail.Email },
                "Login Credentials",
                _mail.GetEmailTemplate("newAdmin", welcomeMail));


            bool sendResult = await _mail.SendAsync(mailData, new CancellationToken());

            if (sendResult)
            {
                return StatusCode(StatusCodes.Status200OK, "Mail has successfully been sent using template.");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured. The Mail could not be sent.");
            }
        }

        [HttpPost("CreatePassword")]
        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        [HttpPost("ForgotPasswordEmail")]
        public async Task<IActionResult> ForgotPasswordEmail(ForgotPasswordVM ForgotPassRequest)
        {

            //System.Web.Security.Membership.GeneratePassword(int length, int numberOfNonAlphanumericCharacters)
            string newPassword = CreatePassword(12);
            var user = await _userManager.FindByEmailAsync(ForgotPassRequest.Email);
            if (user == null) 
            {
                return NotFound("User does not exist");
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetResult = await _userManager.ResetPasswordAsync(user, token, newPassword);


            // Create MailData object
            MailData mailData = new MailData( 
                new List<string> { ForgotPassRequest.Email },
                "Forgot Password",
                _mail.GetEmailTemplate("ForgotPass", new NewClientMail { Email = ForgotPassRequest.Email, Password = newPassword} ));


            bool sendResult = await _mail.SendAsync(mailData, new CancellationToken());

            if (sendResult)
            {
                return Ok("Email has been sent!");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured. The Mail could not be sent.");
            }
        }

    }
}
