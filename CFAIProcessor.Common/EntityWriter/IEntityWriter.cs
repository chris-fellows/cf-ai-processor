using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.EntityWriter
{
    /// <summary>
    /// Writes entities
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IEntityWriter<TEntity>
    {
        void Write(IEnumerable<TEntity> entities);
    }
}
