using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DreamLeague.Models
{
    [Table("ManagerImages", Schema = "DreamLeague")]
    public class ManagerImage
    {
        [Key]
        [ForeignKey("Manager")]
        public int ManagerId { get; set; }

        public byte[] Image { get; set; }

        public virtual Manager Manager { get; set; }

        public ManagerImage() { }

        public ManagerImage(byte[] image)
        {
            Image = image;
        }

        public void SetImage(byte[] image)
        {
            Image = image;
        }
    }
}