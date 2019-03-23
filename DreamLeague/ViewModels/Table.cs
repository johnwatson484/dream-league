using DreamLeague.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DreamLeague.ViewModels
{
    [Serializable]
    public class Table
    {
        public string Name { get; set; }

        public int GroupId { get; set; }

        public List<TableRow> TableRows { get; set; }

        public void Order()
        {
            TableRows = TableRows.OrderByDescending(x => x.Points).ThenByDescending(x => x.GD).ThenByDescending(x => x.GF).ThenBy(x => x.Manager).ToList();

            for (int i = 1; i <= TableRows.Count; i++)
            {
                TableRows[i - 1].Position = i;
            }
        }

        public Table()
        {
            TableRows = new List<TableRow>();
        }

        public Table(string name, int groupId, List<Manager> managers = null) : this()
        {
            Name = name;
            GroupId = groupId;

            if(managers != null)
            {
                foreach(var manager in managers)
                {
                    TableRows.Add(new TableRow(manager.ManagerId, manager.Name, 0, 0, 0, 0, 0, 0, 0));
                }

                Order();
            }
        }
    }

    public class TableRow
    {
        [Display(Name = "Pos")]
        public int Position { get; set; }

        public int ManagerId { get; set; }

        public string Manager { get; set; }

        [Display(Name = "P")]
        public int Played { get; set; }

        [Display(Name = "W")]
        public int Won { get; set; }

        [Display(Name = "D")]
        public int Drawn { get; set; }

        [Display(Name = "L")]
        public int Lost { get; set; }

        [Display(Name = "GF")]
        public int GF { get; set; }

        [Display(Name = "GA")]
        public int GA { get; set; }

        [Display(Name = "GD")]
        public int GD
        {
            get
            {
                return GF - GA;
            }
        }

        [Display(Name = "Pts")]
        public int Points { get; set; }

        public TableRow() { }

        public TableRow(int managerId, string manager, int played, int won, int drawn, int lost, int gf, int ga, int points)
        {
            ManagerId = managerId;
            Manager = manager;
            Played = won + drawn + lost;
            Won = won;
            Drawn = drawn;
            Lost = lost;
            GF = gf;
            GA = ga;
            Points = points;
        }
    }
}