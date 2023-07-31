using System.ComponentModel.DataAnnotations;
namespace TrackwiseAPI.Models.Password
{
    public class ResetPassword
    {
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
    }
}
