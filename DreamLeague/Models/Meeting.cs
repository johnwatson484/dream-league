using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DreamLeague.Models
{
    [Table("Meetings", Schema = "DreamLeague")]
    public class Meeting
    {
        public int MeetingId { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:HH:mm - dd/MM/yyyy}")]
        public DateTime Date { get; set; }

        [Required]
        public string Location { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public string Link
        {
            get
            {
                return string.Format("https://www.google.co.uk/maps/place/{0},{1}", Latitude, Longitude);
            }
        }

        public Meeting() { }

        public Meeting(DateTime date, string location, double longitude, double latitude)
        {
            Date = date;
            Location = location;
            Longitude = longitude;
            Latitude = latitude;
        }
    }
}