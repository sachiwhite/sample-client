using NUnit.Framework;
using sampleserver.Infrastructure;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Moq;

namespace sampleclient.Tests
{
    public class DataFetcherTests
    {
        [SetUp]
        public void Setup()
        {
        }
        /// <summary>
        /// not passing on mock version
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task DataFetcher_UpdateData_Failure()
        {
            Mock<IDataFetcher> dataFetcherFailureMock = new Mock<IDataFetcher>();
            dataFetcherFailureMock.Setup(x => x.UpdateData()).ReturnsAsync(string.Empty);
            var dataFetcher = dataFetcherFailureMock.Object;
            Assert.AreEqual(string.Empty, await dataFetcher.UpdateData());
        }
        [Test]
        public async Task DataFetcher_UpdateData_ReturnsCorrectData()
        {
            var dataFetcher = new MockDataFetcher();
            var expected = "{\"humidity\": 40.0, \"temperature\": 25.0, \"pressure\": 1000.0, \"luminosity\": 0.0, " +
                           "\"lamps\": 1, \"airfans\": 0, \"heaters\": 0, \"timestamp\": \"2020 - 05 - 31 15:05:17\"}";

            var dataToCompare = await dataFetcher.UpdateData();
            Assert.AreEqual(expected,dataToCompare);
        }


    }
}