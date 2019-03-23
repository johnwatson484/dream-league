using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DreamLeague.Models
{
    public enum Position
    {
        Defender,
        Midfielder,
        Forward
    }

    [Table("Players", Schema = "League")]
    public class Player
    {
        public int PlayerId { get; set; }

        [Display(Name = "Team")]
        public int TeamId { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Second Name")]
        [Required]
        public string LastName { get; set; }

        [Required]
        public Position Position { get; set; }

        [Display(Name = "Name")]
        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName).Trim();
            }
        }

        [Display(Name = "Name")]
        public string LastNameFirstName
        {
            get
            {
                if (HasFirstName)
                {
                    return string.Format("{0}, {1}", LastName, FirstName);
                }
                else
                {
                    return string.Format("{0}", LastName);
                }
            }
        }

        [Display(Name = "Name")]
        public string LastNameInitial
        {
            get
            {
                if (HasFirstName)
                {
                    return string.Format("{0}, {1}", LastName, FirstName.FirstOrDefault());
                }
                else
                {
                    return string.Format("{0}", LastName);
                }
            }
        }

        public string Details
        {
            get
            {
                if (HasFirstName)
                {
                    return string.Format("{0}, {1} - {2} - {3}", LastName, FirstName, Position, Team.Name);
                }
                else
                {
                    return string.Format("{0} - {1} - {2}", LastName, Position, Team.Name);
                }
            }
        }

        public bool HasFirstName
        {
            get
            {
                if (!string.IsNullOrEmpty(FirstName))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        [Display(Name = "Goals")]
        public int Scored
        {
            get
            {
                return Goals.Where(x => !x.Cup).Count();
            }
        }

        public char ShortPosition
        {
            get
            {
                switch (Position)
                {
                    case Position.Defender:
                        return 'D';
                    case Position.Midfielder:
                        return 'M';
                    case Position.Forward:
                        return 'F';
                    default:
                        return 'X';
                }
            }
        }

        public virtual Team Team { get; set; }

        public virtual ICollection<ManagerPlayer> ManagerPlayers { get; set; }

        public virtual ICollection<Goal> Goals { get; set; }

        public Player()
        {
            Goals = new List<Goal>();
        }

        public Player(string firstName, string lastName, Position position, int teamId)
        {
            FirstName = firstName;
            LastName = lastName;
            Position = position;
            TeamId = teamId;
        }
    }
}