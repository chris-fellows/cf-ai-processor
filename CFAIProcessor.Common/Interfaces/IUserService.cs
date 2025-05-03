using CFAIProcessor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Interfaces
{
    public interface IUserService : IEntityWithIdService<User, string>
    {
        User? ValidateCredentials(string username, string password);
    }
}
