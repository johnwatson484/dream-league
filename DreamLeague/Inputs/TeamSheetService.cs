using System;
using System.IO;
using System.Linq;
using System.Web;

namespace DreamLeague.Inputs
{
    public class TeamSheetService : ITeamSheetService, IUploadFile
    {
        readonly ITeamSheetReader teamSheetReader;
        readonly string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads");

        private const string fileMask = "TeamSheet_*";

        public TeamSheetService(ITeamSheetReader teamSheetReader)
        {
            this.teamSheetReader = teamSheetReader;
        }

        public TeamSheet Get(string fileName = null)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                DirectoryInfo directory = new DirectoryInfo(path);

                FileInfo file = directory.GetFiles(fileMask).OrderByDescending(x => x.Name).FirstOrDefault();

                if (file != null)
                {
                    fileName = file.Name;
                }
            }

            if (fileName != null)
            {
                try
                {
                    return teamSheetReader.Read(Path.Combine(path, fileName));
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        public DateTime? LastUpload()
        {
            DirectoryInfo directory = new DirectoryInfo(path);

            FileInfo file = directory.GetFiles(fileMask).OrderByDescending(x => x.Name).FirstOrDefault();

            if (file != null)
            {
                return file.CreationTime;
            }
            else
            {
                return null;
            }
        }

        public void Upload(HttpPostedFileBase file)
        {
            string filePath = Path.Combine(path, string.Format("TeamSheet_{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss")));

            file.SaveAs(filePath);
        }

        public void DeleteAll()
        {
            DirectoryInfo directory = new DirectoryInfo(path);

            var files = directory.GetFiles(fileMask);

            foreach (var file in files)
            {
                File.Delete(file.FullName);
            }
        }
    }
}