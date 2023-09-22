namespace TrackwiseAPI.Models.Email
{
    public class TwoFactor
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? twoFactorOTP { get; set; }
    }
}
