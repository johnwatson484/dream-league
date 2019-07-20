using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DreamLeague.Models
{
    [Table("Conceded", Schema = "Results")]
    public class Concede
    {
        public int ConcedeId { get; set; }

        [Display(Name = "Team")]
        public int TeamId { get; set; }

        [Display(Name = "Game Week")]
        public int GameWeekId { get; set; }

        [Display(Name = "Manager")]
        public int ManagerId { get; set; }

        public bool Substitute { get; set; }

        public bool Cup { get; set; }

        public virtual Team Team { get; set; }

        public virtual GameWeek GameWeek { get; set; }

        public virtual Manager Manager { get; set; }

        public Concede() { }

        public Concede(int teamId, int gameWeekId, int managerId, bool substitute = false, bool cup = false)
        {
            TeamId = teamId;
            GameWeekId = gameWeekId;
            ManagerId = managerId;
            Substitute = substitute;
            Cup = cup;
        }
    }
}