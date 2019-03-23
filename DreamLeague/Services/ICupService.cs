using DreamLeague.ViewModels;

namespace DreamLeague.Services
{
    public interface ICupService
    {
        CupViewModel GetData(int cupId);
    }
}