namespace DreamLeague.ViewModels
{
    public class ManagerCupWeek
    {
        public int ManagerId { get; set; }

        public int GameWeekId { get; set; }

        public ManagerCupWeek() { }

        public ManagerCupWeek(int managerId, int gameWeekId)
        {
            ManagerId = managerId;
            GameWeekId = gameWeekId;
        }
    }
}