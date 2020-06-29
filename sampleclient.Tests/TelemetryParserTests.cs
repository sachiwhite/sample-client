using Moq;
using Ninject;
using NUnit.Framework;
using sampleserver.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
            dataFetcherFailureMock.Setup(x => x.UpdateData()).ReturnsAsync(string.Empty);
            dataFetcherFailure = dataFetcherFailureMock.Object;
        }
        [Test]
        public async Task TelemetryParser_UpdateData_Success()
        {
            IKernel kernel = new StandardKernel();
            kernel.Bind<IDataFetcher>().To<MockDataFetcher>();
            kernel.Bind<IDataSaver>().To<CSVDataSaver>();
            IDataFetcher dataFetcher = kernel.Get<IDataFetcher>();
            IDataSaver dataSaver = kernel.Get<IDataSaver>();

            var telemetryParser = new TelemetryParser(dataFetcher, dataSaver);

            bool result = await telemetryParser.UpdateData();
            Assert.AreEqual(true,result);
        }
        [Test]
        public async Task TelemetryParser_ParseTimestamp()
        {
           
            IKernel kernel = new StandardKernel();
            kernel.Bind<IDataFetcher>().To<MockDataFetcher>();
            kernel.Bind<IDataSaver>().To<CSVDataSaver>();
            IDataFetcher dataFetcher = kernel.Get<IDataFetcher>();
            IDataSaver dataSaver = kernel.Get<IDataSaver>();
           
            var telemetryParser = new TelemetryParser(dataFetcher,dataSaver);
            
            await telemetryParser.UpdateData();
            
            var actualDate = await telemetryParser.GetTimestamp();
            var expectedDate = new DateTime(2020, 5, 31, 15, 05, 17);
            Assert.AreEqual(expectedDate, actualDate);
        }
        [Test]
        public async Task TelemetryParser_ParseNumericValues()
        {
            IKernel kernel = new StandardKernel();
            kernel.Bind<IDataFetcher>().To<MockDataFetcher>();
            kernel.Bind<IDataSaver>().To<CSVDataSaver>();
            IDataFetcher dataFetcher = kernel.Get<IDataFetcher>();
            IDataSaver dataSaver = kernel.Get<IDataSaver>();
            var telemetryParser = new TelemetryParser(dataFetcher,dataSaver);
            await telemetryParser.UpdateData();
            var actualValues = telemetryParser.FetchNumericData();
            var expectedNumericValues = new Dictionary<string, double>()
            {
                {"Humidity",40},
                {"Pressure",1000},
                {"Light_intensity",0},
                {"No_of_lamps",1},
                {"Temperature",25},
                {"No_of_airfans",0},	
                {"No_of_heaters",0}
                    
            };
            CollectionAssert.AreEquivalent(expectedNumericValues, actualValues);
        }
        
        [Test]
        public async Task TelemetryParser_ParseTimestamp_Failure()
        {
            var telemetryParser = new TelemetryParser(dataFetcherFailure, new CSVDataSaver());
            await telemetryParser.UpdateData();
            Assert.AreEqual(null, await telemetryParser.GetTimestamp());
        }
        [Test]
        public async Task TelemetryParser_ParseNumericValues_Failure()
        {
            var telemetryParser = new TelemetryParser(dataFetcherFailure, new CSVDataSaver());
            await telemetryParser.UpdateData();
            var expectedNumericValues = new Dictionary<string, double>()
            {
                {"Humidity",0},
                {"Pressure",0},
                {"Light_intensity",0},
                {"No_of_lamps",0},
                {"Temperature",0},
                {"No_of_airfans",0},	
                {"No_of_heaters",0}
                    
            };
            CollectionAssert.AreEquivalent(expectedNumericValues, telemetryParser.FetchNumericData());
        }
        
        [Test]
        public async Task TelemetryParser_UpdateData_Failure()
        {
            var telemetryParser = new TelemetryParser(dataFetcherFailure, new CSVDataSaver());
            bool result = await telemetryParser.UpdateData();
            Assert.AreEqual(false, result);
        }
       
    }
}
