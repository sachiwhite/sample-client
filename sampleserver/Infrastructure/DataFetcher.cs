﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace sampleserver.Infrastructure
{
    public class DataFetcher :IDataFetcher
    {
        HtmlWeb htmlWeb;
        private readonly string RequestUri = "192.168.1.100:2137/telemetry";
        public DataFetcher()
        {
            htmlWeb = new HtmlWeb();
        }
        public List<string> UpdateData()
        {
            HtmlDocument document = new HtmlDocument();
            try
            {
                document = htmlWeb.Load(new UriBuilder(RequestUri).Uri);
            }
            catch (Exception ex)
            {
                //todo: display error message
                return new List<string>();
            }

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
    }
}
