using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DreamLeague.Models
{
    [Table("Emails", Schema ="DreamLeague")]
    public class Email
    {
        public int EmailId { get; set; }

        public int ManagerId { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Address { get; set; }

        public virtual Manager Manager { get; set; }
    }
}