using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Fklib.ApiRRLog.Models;

namespace Fklib.ApiRRLog.Service.Tests
{
    [TestClass()]
    public class ScopeLoggerServiceTests
    {
        [TestMethod()]
        public void LogApiTest()
        {
            // Arrange
            var scopeLoggerService = new ScopeLoggerService();

            // Act
            var jsonLog = scopeLoggerService.LogApi(null, new RootRequest(), new RootResponse(), 10, "127.0.0.1");

            // Assert
            dynamic log = JObject.Parse(jsonLog);
            string logLevel = log.LogLevel;

            Assert.AreEqual(logLevel, "None");
        }
    }
}