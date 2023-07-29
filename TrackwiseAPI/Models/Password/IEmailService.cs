namespace TrackwiseAPI.Models.Password
{
    public interface IEmailService
    {
        Task SendPasswordResetEmailAsync(string email, string encodedToken);
        //Task TESTSendPasswordResetEmailAsync(string email, string token);
    }
}
