using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmHub.Models
{
    public class Report
    {
        [Key]
        public int Id { get; set; }
        public double Yield { get; set; }
        public string Notes { get; set; }
        public bool IsDeleted { get; set; }
        [ForeignKey(nameof(Season))]
        public int SeasonId { get; set; }
        public virtual Season? Season { get; set; }
    }
}
