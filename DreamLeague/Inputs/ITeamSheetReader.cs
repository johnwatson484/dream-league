using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamLeague.Inputs
{
    public interface ITeamSheetReader
    {
        TeamSheet Read(string location);
    }
}
