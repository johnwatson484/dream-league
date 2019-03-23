using DreamLeague.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DreamLeague.Inputs
{
    public class PlayerList
    {
        public List<PlayerListPlayer> Players { get; set; }

        public PlayerList()
        {
            Players = new List<PlayerListPlayer>();
        }
    }

    public class PlayerListPlayer
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Position { get; set; }

        public string Team { get; set; }

        public PlayerListPlayer() { }

        public PlayerListPlayer(string firstName, string lastName, string position, string team)
        {
            FirstName = firstName;
            LastName = lastName;
            Position = position;
            Team = team;
        }

        public string FirstNameUpdated
        {
            get
            {
                if (string.IsNullOrEmpty(FirstName))
                {
                    return null;
                }
                if (!string.IsNullOrEmpty(FirstName) && (LastName == "DEF" || LastName == "MID" || LastName == "FWD" || LastName == "GK" || string.IsNullOrEmpty(LastName)))
                {
                    return null;
                }
                return FirstName;

            }
        }

        public string LastNameUpdated
        {
            get
            {
                if (string.IsNullOrEmpty(LastName) && !string.IsNullOrEmpty(FirstName))
                {
                    return FirstName;
                }
                if (!string.IsNullOrEmpty(FirstName) && (LastName == "DEF" || LastName == "MID" || LastName == "FWD" || LastName == "GK"))
                {
                    return FirstName;
                }
                return LastName;
            }
        }

        public Position? PositionUpdated
        {
            get
            {
                switch(Position)
                {
                    case "DEF":
                        return Models.Position.Defender;                        
                    case "MID":
                        return Models.Position.Midfielder;
                    case "FWD":
                        return Models.Position.Forward;
                    default:
                        return null;                        
                }
            }
        }
    }
}