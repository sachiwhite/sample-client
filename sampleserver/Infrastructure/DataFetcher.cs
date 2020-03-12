using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sampleserver.Infrastructure
{
    public class DataFetcher
    {
        HtmlWeb htmlWeb;
        
        private readonly string RequestUri = "127.0.0.1:2137/telemetry";
        public DataFetcher()
        {
            htmlWeb = new HtmlWeb();
            
        }
        private List<String> FetchData()
        {
            
            htmlWeb = new HtmlWeb();
            HtmlDocument document = htmlWeb.Load(new UriBuilder(RequestUri).Uri);
            document.OptionWriteEmptyNodes = true;
            var text = document.DocumentNode.SelectSingleNode("//hr");
            List<string> textToParse = new List<string>();
            while (text.NextSibling != null)
            {
                textToParse.Add(text.NextSibling.InnerText);
                text = text.NextSibling;
            }
            return textToParse;

        }

        public Dictionary<string, double> GetFetchNumericData()
        {
            return FetchNumericData(FetchData());
        }

        private Dictionary<string, double> FetchNumericData(List<string> textToParse)
        {
            var DataFetched = new Dictionary<string, double>();
            for (int i = 0; i < textToParse.Count; i++)
            {
                var toProcess = textToParse[i];
                toProcess = new string(toProcess.SkipWhile(c => !char.IsLetterOrDigit(c)).ToArray());
                var name = new string(toProcess.TakeWhile(c => char.IsLetter(c)).ToArray());
                var number = new string(toProcess.SkipWhile(c => !char.IsNumber(c)).ToArray());
                if (double.TryParse(number, out double num))
                    DataFetched.Add(name, num);
            }

            return DataFetched;
        }
    }
}
