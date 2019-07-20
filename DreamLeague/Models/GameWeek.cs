using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DreamLeague.Models
{
    [Table("GameWeeks", Schema = "DreamLeague")]
    public class GameWeek
    {
        public int GameWeekId { get; set; }

        [Display(Name = "Game Week")]
        [Required]
        public int Number { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Start { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime End
        {
            get
            {
                return Start.AddDays(7);
            }
        }

        public bool Complete { get; set; }

        public string Details
        {
            get
            {
                return string.Format("{0} - {1:dd/MM/yyyy} - {2:dd/MM/yyyy}", Number, Start, End);
            }
        }

        public string Title
        {
            get
            {
                return string.Format("Game Week {0}", Number);
            }
        }

        public string ShortTitle
        {
            get
            {
                return string.Format("GW{0}", Number);
            }
        }

        public GameWeek()
        {
        }

        public GameWeek(int number, DateTime start) : this()
        {
            Number = number;
            Start = start;
        }


        public void SetComplete()
        {
            Complete = true;
        }

        public void SetIncomplete()
        {
            Complete = false;
        }
    }
}