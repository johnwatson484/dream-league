using System.Collections.Generic;
using DreamLeague.ViewModels;

namespace DreamLeague.Services
{
    public interface ISearchService
    {
        List<Search> Search(string prefix);
    }
}