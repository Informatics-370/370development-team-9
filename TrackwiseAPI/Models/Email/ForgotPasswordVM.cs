using System.ComponentModel.DataAnnotations;

namespace TrackwiseAPI.Models.Email
{
    public class ForgotPasswordVM
    {
        [Required]
        public string Email { get; set; }
    }
}
