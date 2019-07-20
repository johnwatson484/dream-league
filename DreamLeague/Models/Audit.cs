using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DreamLeague.Models
{
    [Table("Audit", Schema = "DreamLeague")]
    public class Audit
    {
        public int AuditId { get; set; }

        public DateTime Date { get; set; }

        public string User { get; set; }

        public string Area { get; set; }

        public string Action { get; set; }

        public int? GameWeekId { get; set; }

        public string Description { get; set; }

    }
}