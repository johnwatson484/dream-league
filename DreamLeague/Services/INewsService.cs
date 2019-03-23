using System.Collections.Generic;
using DreamLeague.Models;

namespace DreamLeague.Services
{
    public interface INewsService
    {
        List<News> Get();
    }
}