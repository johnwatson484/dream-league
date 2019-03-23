using System.Web.Mvc;
using DreamLeague.ViewModels;

namespace DreamLeague.Services
{
    public interface IEmailService
    {
        void Send(GameWeekSummary gameWeekSummary, ControllerContext context);

        string GetAll();
    }
}