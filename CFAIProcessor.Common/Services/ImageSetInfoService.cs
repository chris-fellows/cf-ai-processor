using CFAIProcessor.Interfaces;
using CFAIProcessor.Models;
using CFAIProcessor.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Services
{
    public class ImageSetInfoService : IImageSetInfoService
    {
        private readonly string _folder;

        public ImageSetInfoService(string folder)
        {
            _folder = folder;
        }

        public List<ImageSetInfo> GetAll()
        {
            var list = new List<ImageSetInfo>();

            foreach (var file in Directory.GetFiles(_folder, "*.zip"))
            {                
                // Add image set info
                var imageSetInfo = new ImageSetInfo()
                {
                    Id = Path.GetFileName(file),
                    Name = Path.GetFileNameWithoutExtension(file),                    
                    DataSource = file,
                };

                list.Add(imageSetInfo);
            }

            return list.OrderBy(ds => ds.Name).ToList();
        }

        public ImageSetInfo? GetById(string id)
        {
            var file = Path.Combine(_folder, id);
            if (File.Exists(file))
            {              
                // Add dataset info
                var imageSetInfo = new ImageSetInfo()
                {
                    Id = Path.GetFileName(file),
                    Name = Path.GetFileNameWithoutExtension(file),                  
                    DataSource = file,
                };

                return imageSetInfo;
            }

            return null;
        }

        public void Add(ImageSetInfo imageSetInfo, string tempFile)
        {
            if (!String.IsNullOrEmpty(tempFile))
            {
                File.Move(tempFile, imageSetInfo.DataSource);
            }
        }
    }
}
