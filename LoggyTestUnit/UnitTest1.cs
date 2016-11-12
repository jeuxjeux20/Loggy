using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LoggyTestUnit;
using Loggy;
using System.Linq;
namespace LoggyTestUnit
{
    [TestClass]
    public class UnitTest1 : Loggy.Program
    {
        [TestMethod]
        public void TestMethod1()
        {
            int count = 0;
            try
            {
                count = _client.Servers.Count();
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
