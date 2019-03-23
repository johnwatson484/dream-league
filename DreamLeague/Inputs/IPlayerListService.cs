using System.Collections.Generic;
using System.Web;

namespace DreamLeague.Inputs
{
    public interface IPlayerListService
    {
        string Upload(HttpPostedFileBase file);
    }
}