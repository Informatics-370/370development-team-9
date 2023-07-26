using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrackwiseAPI.Models.Email;
using TrackwiseAPI.Models.Repositories;
using System.Collections.Generic;

namespace TrackwiseAPI.Models.Email
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mail;

        public MailController(IMailService mail)
        {
            _mail = mail;
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

        [HttpPost("sendemailusingtemplate")]
        public async Task<IActionResult> SendEmailUsingTemplate(NewClientMail welcomeMail)
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


        [HttpPost("ForgotPasswordEmail")]
        public async Task<IActionResult> ForgotPasswordEmail(NewClientMail ForgotPassMail)
        {
            // Create MailData object
            MailData mailData = new MailData(
                new List<string> { ForgotPassMail.Email },
                "Login Credentials",
                _mail.GetEmailTemplate("ForgotPass", ForgotPassMail));


            bool sendResult = await _mail.SendAsync(mailData, new CancellationToken());

            if (sendResult)
            {
                return Ok(sendResult);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured. The Mail could not be sent.");
            }
        }

    }
}
