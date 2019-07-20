using System;

namespace DreamLeague.Inputs
{
    public interface ITeamSheetService : IUploadFile
    {
        TeamSheet Get(string fileName = null);

        DateTime? LastUpload();

        void DeleteAll();
    }
}
