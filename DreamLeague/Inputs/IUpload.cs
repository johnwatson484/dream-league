using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DreamLeague.Inputs
{
    public interface IUploadFile
    {
        void Upload(HttpPostedFileBase file);
    }
}
