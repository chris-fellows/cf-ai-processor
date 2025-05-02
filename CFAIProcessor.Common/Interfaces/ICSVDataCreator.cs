using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Interfaces
{
    public interface ICSVDataCreator
    {
        void Create(string dataFile, string configFile, char delimiter, int maxRecords, string[]? columnNames);
    }
}
