using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DreamLeague.Inputs
{
    public class CSVPlayerListReader : IPlayerListReader
    {
        private PlayerList playerList;
        
        public PlayerList Read(string filePath)
        {
            playerList = new PlayerList();

            using (var reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    string firstName = values[0].Trim();
                    string secondName = values[1].Trim();
                    string position = values[2].Trim();
                    string team = values[3].Trim();

                    PlayerListPlayer player = new PlayerListPlayer(firstName, secondName, position, team);
                    playerList.Players.Add(player);
                }
            }

            return playerList;
        }
    }
}