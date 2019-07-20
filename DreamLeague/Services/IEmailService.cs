using DreamLeague.ViewModels;
using System.Web.Mvc;

namespace DreamLeague.Services
{
    public interface IEmailService
    {
        void Send(GameWeekSummary gameWeekSummary, ControllerContext context);

        string GetAll();
    }
}