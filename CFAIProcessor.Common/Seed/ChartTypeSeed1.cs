using CFAIProcessor.EntityReader;
using CFAIProcessor.Models;

namespace CFAIProcessor.Seed
{
    public class ChartTypeSeed1 : IEntityReader<ChartType>
    {
        public IEnumerable<ChartType> Read()
        {
            var list = new List<ChartType>()
            {
                new ChartType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "3D Scatter"                   
                },
                new ChartType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "2D Scatter"
                },
                new ChartType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Bar chart"
                },
                new ChartType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Line chart"
                }
            };

            return list;
        }
    }
}
