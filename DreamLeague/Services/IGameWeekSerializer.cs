namespace DreamLeague.Services
{
    public interface IGameWeekSerializer<T> where T : class
    {
        void Serialize(T gameWeekSummary, int gameWeekId, string prefix);

        T DeSerialize(int gameWeek, string prefix);

        void DeleteAll(string prefix);
    }
}
