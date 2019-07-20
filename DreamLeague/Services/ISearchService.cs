using DreamLeague.ViewModels;
using System.Collections.Generic;

namespace DreamLeague.Services
{
    public interface ISearchService
    {
        List<Search> Search(string prefix);
    }
}