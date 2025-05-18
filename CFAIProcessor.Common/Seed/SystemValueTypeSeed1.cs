using CFAIProcessor.Constants;
using CFAIProcessor.EntityReader;
using CFAIProcessor.Models;

namespace CFAIProcessor.Seed
{
    public class SystemValueTypeSeed1 : IEntityReader<SystemValueType>
    {
        public IEnumerable<SystemValueType> Read()
        {
            var list = new List<SystemValueType>()
            {
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = SystemValueTypeNames.AuditEventId                   
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = SystemValueTypeNames.DataSetInfoId
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = SystemValueTypeNames.ErrorMessage
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = SystemValueTypeNames.Notes
                },
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = SystemValueTypeNames.UserId
                },
            };

            return list;
        }
    }
}
