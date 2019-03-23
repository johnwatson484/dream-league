using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DreamLeague.ViewModels;
using System.IO;
using System.Xml.Serialization;

namespace DreamLeague.Services
{
    public class XMLGameWeekSerializer<T> : IGameWeekSerializer<T> where T : class
    {   
        static string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "XML");
        
        public T DeSerialize(int gameWeekId, string prefix)
        {
            if (gameWeekId == 0)
            {
                return null;
            }          

            string filePath = Path.Combine(path, string.Format("{0}_{1}.xml", prefix, gameWeekId));

            if (File.Exists(filePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                using (TextReader reader = new StreamReader(filePath))
                {
                    return (T)serializer.Deserialize(reader);
                }
            }

            return null;
        }

        public void Serialize(T gameWeekSummary, int gameWeekId, string prefix)
        {
            string filePath = Path.Combine(path, string.Format("{0}_{1}.xml", prefix, gameWeekId));

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (TextWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, gameWeekSummary);
            }
        }

        public void DeleteAll(string prefix)
        {
            DirectoryInfo directory = new DirectoryInfo(path);

            foreach (FileInfo file in directory.GetFiles(string.Format("{0}_*.xml", prefix)))
            {
                file.Delete();
            }
        }
    }
}