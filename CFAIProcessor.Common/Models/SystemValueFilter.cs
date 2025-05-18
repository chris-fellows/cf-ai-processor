using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Models
{
    public class SystemValueFilter
    { 
        /// <summary>
      /// System value type
      /// </summary>
        public string TypeId { get; set; } = String.Empty;

        /// <summary>
        /// System values
        /// </summary>
        public List<string> Values { get; set; } = new();
    }
}
