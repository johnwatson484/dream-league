using DreamLeague.Models;
using OfficeOpenXml;
using System.IO;

namespace DreamLeague.Inputs
{
    public class XLSXTeamSheetReader : ITeamSheetReader
    {
        private ExcelPackage package;
        private ExcelWorksheet sheet1;
        private TeamSheet teamSheet;

        public TeamSheet Read(string filePath)
        {
            teamSheet = new TeamSheet();

            package = new ExcelPackage(new FileInfo(filePath));
            sheet1 = package.Workbook.Worksheets[1];

            int totalManagers = 0;

            for (int i = 2; i < sheet1.Dimension.End.Column; i++)
            {
                var manager = sheet1.Cells[1, i].Value;
                if (manager != null)
                {
                    AddTeam(manager.ToString().Trim(), 1, i);

                    totalManagers++;
                }

                var manager2 = sheet1.Cells[36, i].Value;
                if (manager2 != null)
                {
                    AddTeam(manager2.ToString().Trim(), 36, i);

                    totalManagers++;
                }
            }

            if (totalManagers == 0)
            {
                return null;
            }
            return teamSheet;

        }

        private void AddTeam(string manager, int row, int column)
        {
            const int increment = 2;
            row += 3;

            TeamSheetTeam teamSheetTeam = new TeamSheetTeam(manager);

            var goalkeeper1 = sheet1.Cells[row, column].Value;
            if (goalkeeper1 != null)
            {
                TeamSheetGoalKeeper teamsheetGoalKeeper = new TeamSheetGoalKeeper(goalkeeper1.ToString());
                teamSheetTeam.GoalKeepers.Add(teamsheetGoalKeeper);
            }
            row += increment;

            var goalkeeper2 = sheet1.Cells[row, column].Value;
            if (goalkeeper2 != null)
            {
                TeamSheetGoalKeeper teamsheetGoalKeeper = new TeamSheetGoalKeeper(goalkeeper2.ToString(), true);
                teamSheetTeam.GoalKeepers.Add(teamsheetGoalKeeper);
            }
            row += increment;

            var player1 = sheet1.Cells[row, column].Value;
            if (player1 != null)
            {
                TeamSheetPlayer teamsheetPlayer = new TeamSheetPlayer(player1.ToString(), Position.Defender);
                teamSheetTeam.Players.Add(teamsheetPlayer);
            }
            row += increment;

            var player2 = sheet1.Cells[row, column].Value;
            if (player2 != null)
            {
                TeamSheetPlayer teamsheetPlayer = new TeamSheetPlayer(player2.ToString(), Position.Defender);
                teamSheetTeam.Players.Add(teamsheetPlayer);
            }
            row += increment;

            var player3 = sheet1.Cells[row, column].Value;
            if (player3 != null)
            {
                TeamSheetPlayer teamsheetPlayer = new TeamSheetPlayer(player3.ToString(), Position.Defender, true);
                teamSheetTeam.Players.Add(teamsheetPlayer);
            }
            row += increment;

            var player4 = sheet1.Cells[row, column].Value;
            if (player4 != null)
            {
                TeamSheetPlayer teamsheetPlayer = new TeamSheetPlayer(player4.ToString(), Position.Midfielder);
                teamSheetTeam.Players.Add(teamsheetPlayer);
            }
            row += increment;

            var player5 = sheet1.Cells[row, column].Value;
            if (player5 != null)
            {
                TeamSheetPlayer teamsheetPlayer = new TeamSheetPlayer(player5.ToString(), Position.Midfielder);
                teamSheetTeam.Players.Add(teamsheetPlayer);
            }
            row += increment;

            var player6 = sheet1.Cells[row, column].Value;
            if (player6 != null)
            {
                TeamSheetPlayer teamsheetPlayer = new TeamSheetPlayer(player6.ToString(), Position.Midfielder);
                teamSheetTeam.Players.Add(teamsheetPlayer);
            }
            row += increment;

            var player7 = sheet1.Cells[row, column].Value;
            if (player7 != null)
            {
                TeamSheetPlayer teamsheetPlayer = new TeamSheetPlayer(player7.ToString(), Position.Midfielder, true);
                teamSheetTeam.Players.Add(teamsheetPlayer);
            }
            row += increment;

            var player8 = sheet1.Cells[row, column].Value;
            if (player8 != null)
            {
                TeamSheetPlayer teamsheetPlayer = new TeamSheetPlayer(player8.ToString(), Position.Forward);
                teamSheetTeam.Players.Add(teamsheetPlayer);
            }
            row += increment;

            var player9 = sheet1.Cells[row, column].Value;
            if (player9 != null)
            {
                TeamSheetPlayer teamsheetPlayer = new TeamSheetPlayer(player9.ToString(), Position.Forward);
                teamSheetTeam.Players.Add(teamsheetPlayer);
            }
            row += increment;

            var player10 = sheet1.Cells[row, column].Value;
            if (player10 != null)
            {
                TeamSheetPlayer teamsheetPlayer = new TeamSheetPlayer(player10.ToString(), Position.Forward);
                teamSheetTeam.Players.Add(teamsheetPlayer);
            }
            row += increment;

            var player11 = sheet1.Cells[row, column].Value;
            if (player11 != null)
            {
                TeamSheetPlayer teamsheetPlayer = new TeamSheetPlayer(player11.ToString(), Position.Forward);
                teamSheetTeam.Players.Add(teamsheetPlayer);
            }
            row += increment;

            var player12 = sheet1.Cells[row, column].Value;
            if (player12 != null)
            {
                TeamSheetPlayer teamsheetPlayer = new TeamSheetPlayer(player12.ToString(), Position.Forward);
                teamSheetTeam.Players.Add(teamsheetPlayer);
            }
            row += increment;

            var player13 = sheet1.Cells[row, column].Value;
            if (player13 != null)
            {
                TeamSheetPlayer teamsheetPlayer = new TeamSheetPlayer(player13.ToString(), Position.Forward, true);
                teamSheetTeam.Players.Add(teamsheetPlayer);
            }

            teamSheet.Teams.Add(teamSheetTeam);
        }
    }
}