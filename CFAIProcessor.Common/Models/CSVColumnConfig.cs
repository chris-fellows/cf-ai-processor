using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Models
{
    internal class CSVColumnConfig
    {
        public string Name { get; set; } = String.Empty;

        public int MinValue { get; set; }

        public int MaxValue { get; set; }

        public int GetRandomValue(Random random)
        {
            if (MinValue == MaxValue) return MinValue;

            int value = 0;
            do
            {
                value = random.Next(MinValue, MaxValue);

            } while (value < MinValue || value > MaxValue);

            return value;
        }
    }
}
