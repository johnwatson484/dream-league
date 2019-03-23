using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DreamLeague.Inputs
{
    public class XLSXPlayerListReader : IPlayerListReader
    {
        private ExcelPackage package;
        private ExcelWorksheet sheet1;
        private PlayerList playerList;

        public PlayerList Read(string filePath)
        {
            playerList = new PlayerList();

            package = new ExcelPackage(new FileInfo(filePath));
            sheet1 = package.Workbook.Worksheets[1];

            for (int i = 2; i <= sheet1.Dimension.End.Row; i++)
            {
                string firstName = (sheet1.Cells[i, 1].Value ?? string.Empty).ToString();
                string secondName = (sheet1.Cells[i, 2].Value ?? string.Empty).ToString();
                string position = (sheet1.Cells[i, 3].Value ?? string.Empty).ToString();
                string team = (sheet1.Cells[i, 4].Value ?? string.Empty).ToString();

                PlayerListPlayer player = new PlayerListPlayer(firstName, secondName, position, team);
                playerList.Players.Add(player);
            }

            return playerList;
        }
    }
}