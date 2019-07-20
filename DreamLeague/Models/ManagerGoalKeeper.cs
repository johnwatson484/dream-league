using System.ComponentModel.DataAnnotations.Schema;

namespace DreamLeague.Models
{
    [Table("ManagerGoalKeepers", Schema = "DreamLeague")]
    public class ManagerGoalKeeper
    {
        public int ManagerGoalKeeperId { get; set; }

        public int TeamId { get; set; }

        public int ManagerId { get; set; }

        public bool Substitute { get; set; }

        public virtual Team Team { get; set; }

        public string Details
        {
            get
            {
                if (!Substitute)
                {
                    return string.Format("{0} - {1}", Team.Name, Manager.Name);
                }
                else
                {
                    return string.Format("{0} - {1} (Sub)", Team.Name, Manager.Name);
                }
            }
        }

        public virtual Manager Manager { get; set; }

        public void ToggleSubstitute()
        {
            Substitute = !Substitute;
        }

        public ManagerGoalKeeper()
        {
            Substitute = false;
        }

        public ManagerGoalKeeper(int teamId, int managerId) : this()
        {
            TeamId = teamId;
            ManagerId = managerId;
        }
    }
}