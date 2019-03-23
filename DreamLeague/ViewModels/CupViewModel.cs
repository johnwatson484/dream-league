using DreamLeague.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DreamLeague.ViewModels
{
    public class CupViewModel
    {
        public Cup Cup { get; set; }

        public List<GroupTable> Groups { get; set; }

        public List<FixtureResult> Fixtures { get; set; }

        public CupViewModel()
        {
            Groups = new List<GroupTable>();
            Fixtures = new List<FixtureResult>();
        }

        public CupViewModel(Cup cup, List<GroupTable> groupTables = null, List<FixtureResult> fixtures = null) : this()
        {
            Cup = cup;
            Groups = groupTables;
            Fixtures = fixtures;

            foreach(var fixture in Fixtures)
            {
                fixture.SetCupViewModel(this);
            }
        }
    }

    public class GroupTable
    {
        public Group Group { get; set; }

        public Table Table { get; set; }

        public GroupTable() { }

        public GroupTable(Group group, Table table)
        {
            Group = group;
            Table = table;
        }
    }

    public class FixtureResult
    {
        public string Round
        {
            get
            {
                if (CupViewModel.Cup.HasGroupStage && Fixture.Round == 1)
                {
                    return CupViewModel.Groups.Where(x => x.Group.Managers.Exists(g => g.ManagerId == Fixture.HomeManagerId)).Select(x => x.Group.Name).FirstOrDefault();
                }

                return string.Format("R{0}", Fixture.Round);
            }
        }

        public Fixture Fixture { get; set; }

        public int? HomeScore { get; set; }

        public int? AwayScore { get; set; }

        public bool HasResult
        {
            get
            {
                if (HomeScore != null && AwayScore != null)
                {
                    return true;
                }
                return false;
            }
        }

        public FixtureResult() { }

        public FixtureResult(Fixture fixture, int? homeScore = null, int? awayScore = null)
        {
            Fixture = fixture;
            HomeScore = homeScore;
            AwayScore = awayScore;            
        }

        public void SetCupViewModel(CupViewModel cupViewModel)
        {
            CupViewModel = cupViewModel;
        }

        public CupViewModel CupViewModel { get; set; }
    }
}