using System.Web;

namespace DreamLeague.Inputs
{
    public interface IUploadFile
    {
        string Upload(HttpPostedFileBase file);
    }
}
