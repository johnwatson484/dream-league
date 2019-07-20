using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DreamLeague.Models
{
    [Table("History", Schema = "DreamLeague")]
    public class History
    {
        public int HistoryId { get; set; }

        public int Year { get; set; }

        public int Teams { get; set; }

        [Display(Name = "League")]
        public string League1 { get; set; }

        [Display(Name = "Championship")]
        public string League2 { get; set; }

        public string Cup { get; set; }

        [Display(Name = "League Cup")]
        public string LeagueCup { get; set; }

        public string Plate { get; set; }
    }
}