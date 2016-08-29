using Bazuzi.ValueTypeAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServerTrack.Web.Tests
{
    [TestClass]
    public class ServerNameTests
    {
        [TestMethod]
        public void Has_case_insensitive_equality()
        {
            ValueTypeAssertions.HasValueEquality(new ServerName("FOO"), new ServerName("foo"));
            ValueTypeAssertions.HasValueInequality(new ServerName("FOO"), new ServerName("bar"));
        }
    }
}
