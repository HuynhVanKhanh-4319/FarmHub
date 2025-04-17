using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmHub.Models
{
    public class Crop
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public string ApplicationUserId { get; set; } = string.Empty;

        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public virtual ApplicationUser? ApplicationUser { get; set; }
    }
}
