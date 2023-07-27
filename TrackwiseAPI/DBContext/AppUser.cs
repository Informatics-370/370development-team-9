using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TrackwiseAPI.DBContext
{
    public class AppUser : IdentityUser
    {
        public byte[] PasswordHash { get;set; } =new byte[32];
        public byte[] PasswordSalt { get; set; } = new byte[32];
        public string? PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }

    }
}
