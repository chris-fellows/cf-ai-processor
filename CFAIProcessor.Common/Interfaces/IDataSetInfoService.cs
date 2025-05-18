using CFAIProcessor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Interfaces
{
    public interface IDataSetInfoService
    {
        List<DataSetInfo> GetAll();

        DataSetInfo? GetById(string id);

        void Add(DataSetInfo dataSetInfo, string tempFile);
    }
}
