using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmHub.Models
{
    public class Season
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime HarvestDate { get; set; }
        public double Area { get; set; }
        public string Notes { get; set; }
        public bool IsDeleted { get; set; }
        public string ApplicationUserId { get; set; } = string.Empty;

        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public virtual ApplicationUser? ApplicationUser { get; set; }
        public int CropId { get; set; } 

        [ForeignKey("CropId")]
        [ValidateNever]
        public virtual Crop? Crop{ get; set; }

        public virtual ICollection<Report>? Reports { get; set; }
        public virtual ICollection<Schedule>? Schedules { get; set; }
    }
}
