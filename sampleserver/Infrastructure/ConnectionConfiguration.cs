using System;
using System.Collections.Generic;
using System.Text;

namespace sampleserver.Infrastructure
{
    public class ConnectionConfiguration
    {
        public string RequestUri;
        public ConnectionConfiguration()
        {
            RequestUri = "192.168.1.206:80";
        }
        public void ChangeRequestUri(string Uri)
        {
            RequestUri=Uri;
        }
    }
}
