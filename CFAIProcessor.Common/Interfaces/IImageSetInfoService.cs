using CFAIProcessor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Interfaces
{
    public interface IImageSetInfoService
    {
        List<ImageSetInfo> GetAll();

        ImageSetInfo? GetById(string id);

        void Add(ImageSetInfo imageSetInfo, string tempFile);
    }
}
