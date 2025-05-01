using CFAIProcessor.Utilities;

namespace CFAIProcessor.xUnit
{
    public class UnitTest1
    {
        [Fact]
        public void Normalise_Number_Works()
        {
            Assert.Equal(0, NumericUtilities.Normalize(0, 0, 1000, 0, 1));
            Assert.Equal(1, NumericUtilities.Normalize(1000, 0, 1000, 0, 1));
            Assert.Equal(0.5, NumericUtilities.Normalize(5, 0, 10, 0, 1));

            int xxx = 1000;
        }
    }
}