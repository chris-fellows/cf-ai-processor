using CFAIProcessor.Interfaces;
using CFAIProcessor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Services
{
    public class XmlImageClassifyModelService : XmlEntityWithIdAndNameService<ImageClassifyModel, string>, IImageClassifyModelService
    {
        public XmlImageClassifyModelService(string folder) : base(folder,
                                                "ImageClassifyModel.*.xml",
                                              (imageClassifyModel) => $"ImageClassifyModel.{imageClassifyModel.Id}.xml",
                                                (imageClassifyModelId) => $"ImageClassifyModel.{imageClassifyModelId}.xml",
                                                (imageClassifyModel) => imageClassifyModel.Name)
        {

        }
    }
}
