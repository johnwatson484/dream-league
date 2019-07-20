using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DreamLeague.Models
{
    [Table("Managers", Schema = "DreamLeague")]
    public class Manager
    {
        public int ManagerId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Alias { get; set; }

        public virtual List<ManagerGoalKeeper> GoalKeepers { get; set; }

        public virtual List<ManagerPlayer> Players { get; set; }

        public virtual List<Email> Emails { get; set; }

        public virtual List<Group> Groups { get; set; }

        public virtual List<Goal> Goals { get; set; }

        public virtual List<Concede> Conceded { get; set; }

        public virtual ManagerImage Image { get; set; }

        [Display(Name = "Allow Image")]
        public bool AllowImage { get; set; }

        public string EmailDetails
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                foreach (var email in Emails)
                {
                    sb.Append(string.Format("{0}; ", email.Address));
                }

                return sb.ToString();
            }
        }

        public string FirstName
        {
            get
            {
                return Name.Substring(0, Name.IndexOf(" "));
            }
        }

        public Manager()
        {
            GoalKeepers = new List<ManagerGoalKeeper>();
            Players = new List<ManagerPlayer>();
            Emails = new List<Email>();
        }

        public void SetImage(byte[] image)
        {
            if (Image == null)
            {
                Image = new ManagerImage(image);
            }
            else
            {
                Image.SetImage(image);
            }
        }

        public List<Goal> GameWeekGoals(int gameWeekId, bool cup = false)
        {
            return Goals.Where(x => x.GameWeekId == gameWeekId && x.Cup == cup).ToList();
        }

        public List<Concede> GameWeekConceded(int gameWeekId, bool cup = false)
        {
            return Conceded.Where(x => x.GameWeekId == gameWeekId && x.Cup == cup).ToList();
        }

        public string GameWeekResult(int gameWeekId, bool cup = false)
        {
            int goals = GameWeekGoals(gameWeekId, cup).Count();
            int conceded = GameWeekConceded(gameWeekId, cup).Count();

            if (goals > conceded)
            {
                return "W";
            }
            if (goals == conceded)
            {
                return "D";
            }
            return "L";
        }

        public int GameWeekPoints(int gameWeekId, bool cup = false)
        {
            switch (GameWeekResult(gameWeekId, cup))
            {
                case "W":
                    return 3;
                case "D":
                    return 1;
                default:
                    return 0;
            }
        }

        public int GameWeekMargin(int gameWeekId, bool cup = false)
        {
            int goals = GameWeekGoals(gameWeekId, cup).Count();
            int conceded = GameWeekGoals(gameWeekId, cup).Count();

            return goals - conceded;
        }
    }
}