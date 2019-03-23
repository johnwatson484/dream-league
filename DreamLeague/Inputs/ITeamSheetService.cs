using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamLeague.Inputs
{
    public interface ITeamSheetService: IUploadFile
    {
        TeamSheet Get(string fileName = null);

        DateTime? LastUpload();

        void DeleteAll();
    }
}
