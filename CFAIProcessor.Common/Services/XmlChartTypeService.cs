using CFAIProcessor.Interfaces;
using CFAIProcessor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Services
{
    public class XmlChartTypeService : XmlEntityWithIdAndNameService<ChartType, string>, IChartTypeService
    {
        public XmlChartTypeService(string folder) : base(folder,
                                                "ChartType.*.xml",
                                              (chartType) => $"ChartType.{chartType.Id}.xml",
                                                (chartTypeId) => $"ChartType.{chartTypeId}.xml",
                                                (chartType) => chartType.Name)
        {

        }
    }
}
