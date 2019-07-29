using DreamLeague.Utilities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamLeague.Tests.Utilities
{
    [TestFixture]
    public class LevenshteinDistanceTests
    {
        [Test]
        public void Test_Compute_Returns_Distance()
        {
            Assert.AreEqual(1, LevenshteinDistance.Compute("car", "bar"));
        }

        [Test]
        public void Test_Compute_Returns_Distance_Long_Word()
        {
            Assert.AreEqual(3, LevenshteinDistance.Compute("encyclopedia", "edcyclipexia"));
        }

        [Test]
        public void Test_Compute_Returns_Distance_Multi_Word()
        {
            Assert.AreEqual(3, LevenshteinDistance.Compute("the cat in the hat", "the rat on the mat"));
        }

        [Test]
        public void Test_Compute_Returns_Distance_Multi_Length()
        {
            Assert.AreEqual(2, LevenshteinDistance.Compute("great", "get"));
        }

        [Test]
        public void Test_Compute_Returns_Distance_Multi_Length_And_Word()
        {
            Assert.AreEqual(4, LevenshteinDistance.Compute("great goal", "get foals"));
        }

        [Test]
        public void Test_Compute_Returns_Distance_Multi_Length_And_Word_No_Whitespace()
        {
            Assert.AreEqual(4, LevenshteinDistance.Compute("greatgoal", "getfoals"));
        }
    }
}
