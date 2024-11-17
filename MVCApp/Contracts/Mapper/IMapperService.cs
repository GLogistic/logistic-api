namespace Contracts.Mapper
{
    public interface IMapperService
    {
        TDestination Map<TSource, TDestination>(TSource source);
    }
}
