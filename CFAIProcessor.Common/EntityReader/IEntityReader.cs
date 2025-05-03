namespace CFAIProcessor.EntityReader
{
    public interface IEntityReader<TEntityType>
    {
        IEnumerable<TEntityType> Read();
    }
}
