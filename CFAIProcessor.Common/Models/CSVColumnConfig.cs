namespace CFAIProcessor.Models
{
    /// <summary>
    /// Config for CSV column
    /// </summary>
    public class CSVColumnConfig
    {
        /// <summary>
        /// Name in CSV file
        /// </summary>
        public string InternalName { get; set; } = String.Empty;

        /// <summary>
        /// Display name
        /// </summary>
        public string ExternalName { get; set; } = String.Empty;

        /// <summary>
        /// Min value
        /// </summary>
        public int MinValue { get; set; }

        /// <summary>
        /// Max value
        /// </summary>
        public int MaxValue { get; set; }

        /// <summary>
        /// Whether column is a feature for predictions
        /// </summary>
        public bool IsFeature { get; set; }

        /// <summary>
        /// Whether column is a label for predictions
        /// </summary>
        public bool IsLabel { get; set; }

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
