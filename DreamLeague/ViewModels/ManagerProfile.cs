using DreamLeague.Models;

namespace DreamLeague.ViewModels
{
    public class ManagerProfile
    {
        public Manager Manager { get; set; }

        public GameWeekSummary GameWeekSummary { get; set; }

        public Form Form { get; set; }

        public ManagerProfile() { }

        public ManagerProfile(Manager manager, GameWeekSummary gameWeekSummary, Form form)
        {
            Manager = manager;
            GameWeekSummary = gameWeekSummary;
            Form = form;
        }
    }
}