using Orchard.Park.IRepository;
using Orchard.Park.IRepository.Infrastructure;
using Orchard.Park.Repository.Infrastructure;
using SugarEntity.Entities;
using DotNetCore_CDCD_Charging_Model.SugarEntity;

namespace Orchard.Park.Repository
{
    /// <summary>
    /// {{ModelDescription}}
    /// </summary>
    public class {{ModelClassName}}Repository : BaseRepository<{{ModelClassName}}_Sugar>, I{{ModelClassName}}Repository
    {
        private readonly IUnitOfWork _unitOfWork;

        public {{ModelClassName}}Repository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
