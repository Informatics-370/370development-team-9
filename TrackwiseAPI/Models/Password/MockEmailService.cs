namespace TrackwiseAPI.Models.Password
{
    public class MockEmailService // : IEmailService
    {
        public async Task TESTSendPasswordResetEmailAsync(string email, string token)
        {
            // Log the email content to the console (you can also log to a file or other output)
            System.Diagnostics.Debug.WriteLine("Mock Email Service: Password Reset Email");
            System.Diagnostics.Debug.WriteLine($"Recipient: {email}");
            System.Diagnostics.Debug.WriteLine($"Reset Token: {token}");
        }
    }
}
