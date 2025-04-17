using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FarmHub.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string? Name { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public string? CitizenID { get; set; } = string.Empty;
        public string? Image { get; set; } = string.Empty;
        public virtual ICollection<Season> Seasons { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }


    }
}
