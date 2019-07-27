using DreamLeague.Models;
using System.Collections.Generic;

namespace DreamLeague.Tests.Data.Mock
{
    public static class ManagerData
    {
        public static List<Manager> Data()
        {
            return new List<Manager>
            {
                new Manager
                {
                    ManagerId = 1,
                    Name = "John"
                },
                new Manager
                {
                    ManagerId = 1,
                    Name = "Lee"
                }
            };
        }
    }
}

