using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Utilities
{
    internal class NumericUtilities
    {
        public static int RoundDownToNearest(int value, int divisor)
        {
            return (value / divisor) * divisor;
        }
    }
}
