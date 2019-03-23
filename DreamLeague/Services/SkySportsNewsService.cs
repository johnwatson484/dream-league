using DreamLeague.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace DreamLeague.Services
{
    public class SkySportsNewsService : INewsService
    {
        const string url = "https://skysportsapi.herokuapp.com/sky/getnews/football/v1.0/";

        public List<News> Get()
        {
            string json;
            List<News> news = new List<News>();

            using (WebClient client = new WebClient())
            {
                try
                {
                    json = client.DownloadString(url);
                }
                catch (Exception)
                {
                    json = null;
                }
            }

            if (json != null)
            {
                news = JsonConvert.DeserializeObject<List<News>>(json);
            }

            return news;
        }
    }
}