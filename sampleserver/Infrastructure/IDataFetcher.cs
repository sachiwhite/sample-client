using System.Collections.Generic;
using System.Threading.Tasks;

namespace sampleserver.Infrastructure
{
    public interface IDataFetcher
    {
        Task<string> UpdateData();
    }
}
