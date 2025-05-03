using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Interfaces
{
    public interface IEntityWithIdNameService<TEntityType, TEntityIdType> : IEntityWithIdService<TEntityType, TEntityIdType>
    { 
        /// <summary>
        /// Gets entity by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<TEntityType?> GetByNameAsync(string name);
    }
}
