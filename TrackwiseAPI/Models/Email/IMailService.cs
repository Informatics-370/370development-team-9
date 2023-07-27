using TrackwiseAPI.Models.Email;
namespace TrackwiseAPI.Models.Email
{
    public interface IMailService
    {
        Task<bool> SendAsync(MailData mailData, CancellationToken ct);
        string GetEmailTemplate<T>(string emailTemplate, T emailTemplateModel);
    }
}
