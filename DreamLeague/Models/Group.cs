using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DreamLeague.Models
{
    [Table("Groups", Schema = "Cup")]
    public class Group
    {
        public int GroupId { get; set; }

        public int CupId { get; set; }

        public string Name { get; set; }

        [Range(1, 2)]
        [Display(Name = "Group Legs")]
        public int GroupLegs { get; set; }

        [Display(Name = "Teams Advancing")]
        public int TeamsAdvancing { get; set; }

        public Cup Cup { get; set; }

        public virtual List<Manager> Managers { get; set; }

        public Group()
        {
            GroupLegs = 2;
            TeamsAdvancing = 2;
            Managers = new List<Manager>();
        }

        public Group(int cupId) : this()
        {
            CupId = cupId;
        }
    }
}