using System.Linq;
using Loggy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LoggyTestUnit
{
    [TestClass]
    public class UnitTest1 : Program
    {
        [TestMethod]
        public void TestMethod1()
        {
            int count = 0;
            try
            {
                count = Client.Servers.Count();
            }
            catch
            {
                Assert.Fail();
            }
            if (count >0)
            {
                Assert.AreNotEqual(0, count);
            }
            
        }
    }
}
