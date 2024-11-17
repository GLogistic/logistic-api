using BusinessLogic.Base;
using Contracts.Mapper;
using Contracts.Repositories;
using Contracts.Services;
using Entities;

namespace BusinessLogic
{
    public class CargoService : BaseService<Cargo>, ICargoService
    {
        public CargoService(ICargoRepository repository, IMapperService mapperService) : base(repository, mapperService)
        {
        }
    }
}
