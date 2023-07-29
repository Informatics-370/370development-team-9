using System.ComponentModel.DataAnnotations.Schema;
using TrackwiseAPI.Models.Entities;

namespace TrackwiseAPI.Models.Email
{
    public class NewDriverMail
    {
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Driver_Status_ID { get; set; }

    }
}
