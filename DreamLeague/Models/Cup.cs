using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DreamLeague.Models
{
    [Table("Cups", Schema = "Cup")]
    public class Cup
    {
        public int CupId { get; set; }

        public string Name { get; set; }

        [Display(Name="Has Group Stage?")]
        public bool HasGroupStage { get; set; }        
        
        [Range(1,2)]
        [Display(Name = "Knockout Legs")]
        public int KnockoutLegs { get; set; }

        [Range(1, 2)]
        [Display(Name = "Final Legs")]
        public int FinalLegs { get; set; }

        public virtual List<Fixture> Fixtures { get; set; }

        public virtual List<Group> Groups { get; set; }

        public Cup()
        {
            KnockoutLegs = 2;
            FinalLegs = 2;
        }
    }
}