using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DreamLeague.Models
{
    [Table("Goals", Schema = "Results")]
    public class Goal
    {
        public int GoalId { get; set; }

        [Display(Name = "Player")]
        public int PlayerId { get; set; }

        [Display(Name = "Game Week")]
        public int GameWeekId { get; set; }

        [Display(Name = "Manager")]
        public int ManagerId { get; set; }

        public bool Cup { get; set; }

        public virtual Player Player { get; set; }

        public virtual GameWeek GameWeek { get; set; }

        public virtual Manager Manager { get; set; }

        public Goal() { }

        public Goal(int playerId, int gameWeekId, int managerId, bool cup = false)
        {
            PlayerId = playerId;
            GameWeekId = gameWeekId;
            ManagerId = managerId;
            Cup = cup;
        }
    }
}