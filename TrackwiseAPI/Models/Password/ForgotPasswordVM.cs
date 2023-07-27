using System.ComponentModel.DataAnnotations;

namespace TrackwiseAPI.Models.Password
{
    public class ForgotPasswordVM
    {
        [Required]
        public string Email { get; set; }
    }
}
