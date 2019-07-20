using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DreamLeague.Models
{
    [Serializable]
    [Table("Leagues", Schema = "League")]
    public class League
    {
        public int LeagueId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Rank { get; set; }

        public virtual List<Team> Teams { get; set; }
    }
}