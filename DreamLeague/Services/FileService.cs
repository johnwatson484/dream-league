using System;
using System.IO;

namespace DreamLeague.Services
{
    public class FileService : IFileService
    {
        public byte[] GetBytesFromFile(string fileName)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Downloads", fileName);

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            return fileBytes;
        }
    }
}