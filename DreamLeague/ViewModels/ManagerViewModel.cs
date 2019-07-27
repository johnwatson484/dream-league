using DreamLeague.Models;
using System.ComponentModel.DataAnnotations;

namespace DreamLeague.ViewModels
{
    public class ManagerViewModel
    {
        public Manager Manager { get; set; }

        [Display(Name = "Email 1")]
        public Email Email1 { get; set; }

        [Display(Name = "Email 2")]
        public Email Email2 { get; set; }

        public ManagerViewModel()
        {
            Manager = new Manager();
            Email1 = new Email();
            Email2 = new Email();
        }

        public ManagerViewModel(Manager manager) : this()
        {
            Manager = manager;

            int emails = manager.Emails.Count;

            if (emails > 0)
            {
                Email1 = manager.Emails[0];
            }
            if (emails > 1)
            {
                Email2 = manager.Emails[1];
            }
        }
    }
}