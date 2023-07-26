namespace TrackwiseAPI.Models.Password
{
    public interface IEmailService
    {
        Task SendPasswordResetEmailAsync(string email, string token);
        //Task TESTSendPasswordResetEmailAsync(string email, string token);
    }
}
