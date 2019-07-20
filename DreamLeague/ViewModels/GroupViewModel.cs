using DreamLeague.Models;
using System.Collections.Generic;

namespace DreamLeague.ViewModels
{
    public class GroupViewModel
    {
        public Group Group { get; set; }

        public Cup Cup { get; set; }

        public List<ManagerSelection> Managers { get; set; }

        public GroupViewModel()
        {
            Managers = new List<ManagerSelection>();
        }

        public GroupViewModel(Cup cup, List<Manager> managers, Group group = null) : this()
        {
            if (group == null)
            {
                Group = new Group(cup.CupId);
            }
            else
            {
                Group = group;
            }

            Cup = cup;

            foreach (var manager in managers)
            {
                bool selected = false;

                if (Group.Managers.Exists(x => x.ManagerId == manager.ManagerId))
                {
                    selected = true;
                }

                Managers.Add(new ManagerSelection(manager, selected));
            }
        }
    }

    public class ManagerSelection
    {
        public Manager Manager { get; set; }

        public bool Selected { get; set; }

        public ManagerSelection() { }

        public ManagerSelection(Manager manager, bool selected = false)
        {
            Manager = manager;
            Selected = selected;
        }
    }
}