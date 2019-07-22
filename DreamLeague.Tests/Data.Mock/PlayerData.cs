using DreamLeague.Models;
using System.Collections.Generic;

namespace DreamLeague.Tests.Data.Mock
{
    public static class PlayerData
    {
        public static List<Player> Data()
        {
            return new List<Player>
            {
                new Player
                {
                    PlayerId = 1,
                    FirstName = "Adebayo",
                    LastName = "Akinfenwa",
                    Position = Position.Forward,
                    Team = new Team(),
                    ManagerPlayers = new List<ManagerPlayer>()
                },
                new Player
                {
                    PlayerId = 2,
                    FirstName = "Paddy",
                    LastName = "Madden",
                    Position = Position.Forward,
                    Team = new Team(),
                    ManagerPlayers = new List<ManagerPlayer>()
                }
            };
        }
    }
}

