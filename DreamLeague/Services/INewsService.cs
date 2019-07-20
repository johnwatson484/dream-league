using DreamLeague.Models;
using System.Collections.Generic;

namespace DreamLeague.Services
{
    public interface INewsService
    {
        List<News> Get();
    }
}