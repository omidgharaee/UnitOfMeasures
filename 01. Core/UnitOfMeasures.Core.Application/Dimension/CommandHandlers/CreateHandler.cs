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
    public class CreateHandler : ICommandHandler<Create>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDimensionRepository _dimensionRepository;

        public CreateHandler(IUnitOfWork unitOfWork, IDimensionRepository dimensionRepository)
        {
            _unitOfWork = unitOfWork;
            _dimensionRepository = dimensionRepository;
        }
        public void Handle(Create command)
        {
            if (_dimensionRepository.Exists(command.Id))
                throw new InvalidOperationException($"قبلا با شناسه {command.Id} ثبت شده است.");

            Core.Domain.Dimension.Entities.Dimension dimension = new Core.Domain.Dimension.Entities.Dimension(command.Id, command.Name);

            _dimensionRepository.Add(dimension);
            _unitOfWork.Commit();
        }
    }
}
