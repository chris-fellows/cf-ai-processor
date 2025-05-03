using System.Transactions;

namespace CFAIProcessor.Utilities
{
    public static class NumericUtilities
    {
        /// <summary>
        /// Rounds number down to nearest divisor
        /// </summary>
        /// <param name="value"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public static int RoundDownToNearestDivisor(int value, int divisor)
        {
            return (value / divisor) * divisor;
        }

        /// <summary>
        /// Normalizes value to value within the output range.
        /// 
        /// E.g. Value=5, ValueMin=1, ValueMax=10, OutputMin=0, OutputMax=1, returns 0.5
        /// </summary>
        /// <param name="value"></param>
        /// <param name="valueMin"></param>
        /// <param name="valueMax"></param>
        /// <param name="outputMin"></param>
        /// <param name="outputMax"></param>
        /// <returns></returns>
        public static double Normalize(double value, double valueMin, double valueMax, double outputMin, double outputMax)
        {
            return (((value - valueMin) / (valueMax - valueMin)) * (outputMax - outputMin)) + outputMin;
        }

        /// <summary>
        /// Returns random item based on approx proportion that item should be selected.
        /// 
        /// E.g. If we have 3 items and the proportions are 0.60, 0.20, 0.20 then item 0 should be returned approximately
        /// 60% of the time.
        /// 
        /// Selected proportions should total up to 1.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="values"></param>
        /// <param name="minRandomValue"></param>
        /// <param name="maxRandomValue"></param>
        /// <returns></returns>
        public static T GetWeightedRandom<T>(List<T> items, List<double> itemSelectionProportion)
        {            
            if (items.Count == 1) return items.First();

            const double totalRangeSize = Int32.MaxValue;

            // Set number ranges 
            var ranges = new List<int[]>();
            int rangeStart = 0;
            for(int index = 0; index < items.Count; index++)
            {
                if (itemSelectionProportion[index] > 0)
                {
                    // Calculate the range for this item
                    var rangeSize = (int)(itemSelectionProportion[index] * totalRangeSize);
                    var isSame = rangeSize == totalRangeSize;
                    ranges.Add(new int[] { rangeStart, rangeStart + rangeSize - 1 });

                    // Set start point for next item
                    rangeStart += rangeSize;
                }
                else    // Should never be selected, set range to ignore
                {
                    ranges.Add(new[] { -1, -1 });
                }
            }

            // Select a random number within total range
            var random = new Random();
            var randomNumber = random.Next(0, ranges.Where(r => r[0] != -1).Last()[1]);

            // Return the number
            for(var index = 0; index < ranges.Count; index++)
            {
                if (randomNumber >= ranges[index][0] && randomNumber <= ranges[index][1])
                {
                    return items[index];
                }
            }

            throw new ArgumentException("Failed to return random item");
        }
    }
}
