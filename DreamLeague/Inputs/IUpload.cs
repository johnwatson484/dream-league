using System.Web;

namespace DreamLeague.Inputs
{
    public interface IUploadFile
    {
        void Upload(HttpPostedFileBase file);
    }
}
