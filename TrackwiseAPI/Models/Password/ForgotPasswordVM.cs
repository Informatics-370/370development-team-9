using System.ComponentModel.DataAnnotations;

namespace TrackwiseAPI.Models.Password
{
    public class ForgotPasswordVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
