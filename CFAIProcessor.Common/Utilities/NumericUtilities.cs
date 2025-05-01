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
    }
}
