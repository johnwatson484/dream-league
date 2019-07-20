using DreamLeague.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DreamLeague.ViewModels
{
    [Serializable]
    public class GameWeekSummary
    {
        public GameWeek GameWeek { get; set; }

        public List<Score> Scores { get; set; }

        public Table Table { get; set; }

        public List<string> Winners
        {
            get
            {
                return Scores.Where(x => x.Margin == (Scores.Max(s => s.Margin))).Select(x => x.Manager).ToList();
            }
        }

        public List<string> JackPotWinners
        {
            get
            {
                return Scores.Where(x => x.Goals >= 10 && x.Goals == Scores.Max(s => s.Goals)).Select(x => x.Manager).ToList();
            }
        }

        public GameWeekSummary()
        {
            GameWeek = new GameWeek();

            Scores = new List<Score>();

            Table = new Table();
        }

        public GameWeekSummary(GameWeek gameWeek, List<Score> scores, Table table)
        {
            GameWeek = gameWeek;
            Scores = scores;
            Table = table;
        }
    }
}