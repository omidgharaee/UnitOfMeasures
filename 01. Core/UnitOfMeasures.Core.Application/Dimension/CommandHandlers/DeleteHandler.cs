using Framework.Domain.ApplicationServices;
using Framework.Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitOfMeasures.Core.Domain.Dimension.Commands;
using UnitOfMeasures.Core.Domain.Dimension.Data;

namespace UnitOfMeasures.Core.Application.Dimension.CommandHandlers
{
    public class DeleteHandler : ICommandHandler<Delete>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDimensionRepository _dimensionRepository;
        public DeleteHandler(IUnitOfWork unitOfWork, IDimensionRepository dimensionRepository)
        {
            _unitOfWork = unitOfWork;
            _dimensionRepository = dimensionRepository;
        }
        public void Handle(Delete command)
        {
            var dimension = _dimensionRepository.Load(command.Id);
            _dimensionRepository.Remove(dimension);
            _unitOfWork.Commit();
        }
    }
}
