using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Interfaces
{
    public interface IFileSecurityCheckerService
    {          
        Task<bool> ValidateCanUploadImageAsync(byte[] content);
    }
}
