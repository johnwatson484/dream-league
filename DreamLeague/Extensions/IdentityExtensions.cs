using DreamLeague.DAL;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace DreamLeague.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetName(this IIdentity identity)
        {
            DreamLeagueContext db = new DreamLeagueContext();

            var managerId = identity.GetManagerId();

            var manager = db.Managers.AsNoTracking().Where(x => x.ManagerId.ToString() == managerId).FirstOrDefault();

            if (manager != null)
            {
                return manager.Name;
            }
            else
            {
                return identity.Name;
            }
        }

        public static string GetManagerId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("ManagerId");

            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}