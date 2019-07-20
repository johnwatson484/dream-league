using System.ComponentModel.DataAnnotations.Schema;

namespace DreamLeague.Models
{
    [Table("ManagerPlayers", Schema = "DreamLeague")]
    public class ManagerPlayer
    {
        public int ManagerPlayerId { get; set; }

        public int PlayerId { get; set; }

        public int ManagerId { get; set; }

        public Player Player { get; set; }

        public bool Substitute { get; set; }

        public string Details
        {
            get
            {
                return string.Format("{0} - {1} - {2}", Player.LastNameFirstName, Player.Team.Name, Manager.Name);
            }
        }

        public virtual Manager Manager { get; set; }

        public void ToggleSubstitute()
        {
            Substitute = !Substitute;
        }

        public ManagerPlayer()
        {
            Substitute = false;
        }

        public ManagerPlayer(int playerId, int managerId) : this()
        {
            PlayerId = playerId;
            ManagerId = managerId;
        }
    }
}