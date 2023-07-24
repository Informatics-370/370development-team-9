namespace TrackwiseAPI.Models.Password
{
    public class EmailSettings
    {
        public string EmailSender { get; set; }
        public string EmailSenderPassword { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
    }
}
