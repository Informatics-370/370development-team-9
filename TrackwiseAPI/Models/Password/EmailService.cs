using System.Net.Mail;
using System.Net;
using System.Web;
using Microsoft.Extensions.Options;

namespace TrackwiseAPI.Models.Password
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendPasswordResetEmailAsync(string email, string encodedToken)
        {
            var subject = "Password Reset Request";
            var resetPasswordUrl = "https://your-app-url.com/reset-password"; // Replace with your actual reset password URL

            var message = $@"<p>Please click the following link to reset your password:</p>
                 <p><a href=""{resetPasswordUrl}?email={email}&token={encodedToken}"">Reset Password</a></p>";


            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = _emailSettings.EmailSender,
                    Password = _emailSettings.EmailSenderPassword
                };

                smtp.UseDefaultCredentials = false;
                smtp.Credentials = credential;
                smtp.Host = _emailSettings.SmtpHost;
                smtp.Port = _emailSettings.SmtpPort;
                smtp.EnableSsl = true;

                using (var emailMessage = new MailMessage())
                {
                    emailMessage.To.Add(new MailAddress(email));
                    emailMessage.From = new MailAddress(_emailSettings.EmailSender);
                    emailMessage.Subject = subject;
                    emailMessage.Body = message;
                    emailMessage.IsBodyHtml = true;

                    await smtp.SendMailAsync(emailMessage);
                }
            }
        }
    }
}
