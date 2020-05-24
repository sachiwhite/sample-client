using System.Collections.Generic;

namespace sampleserver.Infrastructure
{
    public interface IDataFetcher
    {
        List<string> UpdateData();
    }
}
