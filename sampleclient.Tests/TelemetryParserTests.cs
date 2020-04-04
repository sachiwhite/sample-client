using Moq;
using Ninject;
using NUnit.Framework;
using sampleserver.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace sampleclient.Tests
{
    public class TelemetryParserTests
    {
        Dictionary<string, string> expectedParsedData = new Dictionary<string, string>()
        {
            {"Timestamp", "2020-03-21 13:57:31"},
            {"Temperature", "27.5"},
            {"Humidity", "98.0"},
            {"Pressure", "100.0"},
            {"Light_intensity", "0.4"},
            {"No_of_lamps", "2"},
            {"No_of_airfans", "1"},
            {"No_of_heaters", "0"},
            { "Photo", " TBD "}
        };
        IDataFetcher dataFetcherFailure;

        [SetUp]
        public void Setup()
        {
            Mock<IDataFetcher> dataFetcherFailureMock = new Mock<IDataFetcher>();
            dataFetcherFailureMock.Setup(x => x.UpdateData()).Returns(new List<string>());
            dataFetcherFailure = dataFetcherFailureMock.Object;
        }
        [Test]
        public void TelemetryParser_UpdateData_Success()
        {
            IKernel kernel = new StandardKernel();
            kernel.Bind<IDataFetcher>().To<MockDataFetcher>();
            IDataFetcher dataFetcher = kernel.Get<IDataFetcher>();
            var telemetryParser = new TelemetryParser(dataFetcher);
            telemetryParser.UpdateData();
            CollectionAssert.AreEquivalent(expectedParsedData, telemetryParser.parsedData);
        }
        [Test]
        public void TelemetryParser_ParseTimestamp()
        {
            IKernel kernel = new StandardKernel();
            kernel.Bind<IDataFetcher>().To<MockDataFetcher>();
            IDataFetcher dataFetcher = kernel.Get<IDataFetcher>();
            var telemetryParser = new TelemetryParser(dataFetcher);
            telemetryParser.UpdateData();
            var actualDate = telemetryParser.GetTimestamp();
            var expectedDate = new DateTime(2020, 3, 21, 13, 57, 31);
            Assert.AreEqual(expectedDate, actualDate);
        }
        [Test]
        public void TelemetryParser_ParseNumericValues()
        {
            IKernel kernel = new StandardKernel();
            kernel.Bind<IDataFetcher>().To<MockDataFetcher>();
            IDataFetcher dataFetcher = kernel.Get<IDataFetcher>();
            var telemetryParser = new TelemetryParser(dataFetcher);
            telemetryParser.UpdateData();
            var expectedNumericValues = new Dictionary<string, double>()
            {
                    {"Temperature", 27.5},
                    {"Humidity", 98.0},
                    {"Pressure", 100.0},
                    {"Light_intensity", 0.4},
                    {"No_of_lamps", 2},
                    {"No_of_airfans", 1},
                    {"No_of_heaters", 0},
                    
            };
            CollectionAssert.AreEquivalent(expectedNumericValues, telemetryParser.FetchNumericData());
        }
        [Test]
        public void TelemetryParser_ParsePhotoLink()
        {
            IKernel kernel = new StandardKernel();
            kernel.Bind<IDataFetcher>().To<MockDataFetcher>();
            IDataFetcher dataFetcher = kernel.Get<IDataFetcher>();
            var telemetryParser = new TelemetryParser(dataFetcher);
            telemetryParser.UpdateData();
            Assert.AreEqual(" TBD ", telemetryParser.ParsePhotoLink());
        }
        [Test]
        public void TelemetryParser_ParseTimestamp_Failure()
        {
            var telemetryParser = new TelemetryParser(dataFetcherFailure);
            telemetryParser.UpdateData();
            Assert.AreEqual(null, telemetryParser.GetTimestamp());
        }
        [Test]
        public void TelemetryParser_ParseNumericValues_Failure()
        {
            var telemetryParser = new TelemetryParser(dataFetcherFailure);
            telemetryParser.UpdateData();
            CollectionAssert.AreEquivalent(new Dictionary<string, double>(), telemetryParser.FetchNumericData());
        }
        [Test]
        public void TelemetryParser_ParsePhotoLink_Failure()
        {
            var telemetryParser = new TelemetryParser(dataFetcherFailure);
            telemetryParser.UpdateData();
            Assert.AreEqual(string.Empty, telemetryParser.ParsePhotoLink());
        }
        [Test]
        public void TelemetryParser_UpdateData_Failure()
        {
            var telemetryParser = new TelemetryParser(dataFetcherFailure);
            telemetryParser.UpdateData();
            CollectionAssert.AreEquivalent(new Dictionary<string, string>(), telemetryParser.parsedData);
        }
       
    }
}
