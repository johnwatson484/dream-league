using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DreamLeague.Models
{
    [Table("Fixtures", Schema = "Cup")]
    public class Fixture
    {
        public int FixtureId { get; set; }

        public int CupId { get; set; }

        public int GameWeekId { get; set; }

        [Display(Name = "Home")]
        [ForeignKey("HomeManager")]
        public int HomeManagerId { get; set; }

        [Display(Name = "Away")]
        [ForeignKey("AwayManager")]
        public int AwayManagerId { get; set; }

        [Range(1,10)]
        public int Round { get; set; }

        public virtual Cup Cup { get; set; }

        public virtual GameWeek GameWeek { get; set; }
        
        public virtual Manager HomeManager { get; set; }

        public virtual Manager AwayManager { get; set; }

        public Fixture() { }

        public Fixture(int cupId, int? round = null)
        {
            CupId = cupId;
            Round = round ?? 1;           
        }
    }
}