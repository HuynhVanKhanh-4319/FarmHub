using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmHub.Models
{
    public class Schedule
    {
        [Key]
        public int Id { get; set; }
     
        public DateTime Date { get; set; }
        public string Activity { get; set; }
        public bool IsDeleted { get; set; }
        [Required(ErrorMessage = "Mùa vụ là bắt buộc.")]
        [ForeignKey(nameof(Season))]
        public int SeasonId { get; set; }
        public virtual Season? Season { get; set; }
    }
}
