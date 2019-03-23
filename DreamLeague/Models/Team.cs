using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DreamLeague.Models
{
    [Table("Teams", Schema = "League")]
    public class Team
    {
        public int TeamId { get; set; }

        [Display(Name = "League")]
        public int LeagueId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Alias { get; set; }

        public string Details
        {
            get
            {
                return string.Format("{0} - {1}", Name, League.Name);
            }
        }

        public int ConcededGoals
        {
            get
            {
                return Conceded.Where(x => !x.Cup).Count();
            }
        }

        public virtual League League { get; set; }

        public virtual List<Player> Players { get; set; }

        public virtual ICollection<ManagerGoalKeeper> ManagerGoalKeepers { get; set; }

        public virtual ICollection<Concede> Conceded { get; set; }

        public Team()
        {
            Conceded = new List<Concede>();
        }
    }
}