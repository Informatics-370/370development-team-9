using System.ComponentModel.DataAnnotations;

namespace TrackwiseAPI.Models.ViewModels
{
    public class TwoFactorVM
    {
        [Required]
        [DataType(DataType.Text)]
        public string Code { get; set; }
        public string Username { get; set; }
    }
}
