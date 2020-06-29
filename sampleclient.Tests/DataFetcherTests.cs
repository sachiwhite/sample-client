using NUnit.Framework;
using sampleserver.Infrastructure;
using System.Collections.Generic;
using System.Net;

namespace sampleclient.Tests
{
    public class DataFetcherTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void DataFetcher_UpdateData_Failure()
        {
            var dataFetcher = new DataFetcher();
            CollectionAssert.AreEquivalent(new List<string>(), dataFetcher.UpdateData());
        }
        [Test]
        public void DataFetcher_UpdateData_ReturnsCorrectData()
        {
            var dataFetcher = new DataFetcher();
            var expected = new List<string>()
            {
                "\n        \n        \n        \n        \n\tTimestamp: 2020-03-21 13:57:31 ",
                "\n\tTemperature: 27.5 ",
                "\n\tHumidity: 98.0 ",
                "\n\tPressure: 100.0 ",
                "\n\tLight_intensity: 0.4 ",
                " \n\tNo_of_lamps: 2 ",
                "\n\tNo_of_airfans: 1 ",
                "\n\tNo_of_heaters: 0 ",
                "\n\tPhoto: TBD ",
                "",
                "\n\n    "
            };
            var dataToCompare = dataFetcher.UpdateData();
            CollectionAssert.AreEquivalent(expected, dataToCompare);
        }


    }
}