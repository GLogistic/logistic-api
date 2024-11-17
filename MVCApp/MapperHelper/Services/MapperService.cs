using AutoMapper;
using Contracts.Mapper;

namespace MapperHelper.Services
{
    public class MapperService : IMapperService
    {
        private readonly IMapper _mapper;
        public MapperService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDestination Map<TSource, TDestination>(TSource source) =>
            _mapper.Map<TDestination>(source);
    }
}
