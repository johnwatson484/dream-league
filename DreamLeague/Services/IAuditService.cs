namespace DreamLeague.Services
{
    public interface IAuditService
    {
        void Log(string area, string action, string user, string description, int? gameWeekId = null);
    }
}