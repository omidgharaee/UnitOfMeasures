using Framework.Domain.ApplicationServices;
using Framework.Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitOfMeasures.Core.Domain.Dimension.Commands;
using UnitOfMeasures.Core.Domain.Dimension.Data;

namespace UnitOfMeasures.Core.Application.Unit.CommandHandlers
{
    public class DeleteHandler : ICommandHandler<Core.Domain.Unit.Commands.Delete>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUnitRepository _unitRepository;
        public DeleteHandler(IUnitOfWork unitOfWork, IUnitRepository unitRepository)
        {
            _unitOfWork = unitOfWork;
            _unitRepository = unitRepository;
        }
        public void Handle(Core.Domain.Unit.Commands.Delete command)
        {
            var dimension = _unitRepository.Load(command.Id);
            _unitRepository.Remove(dimension);
            _unitOfWork.Commit();
        }
    }
}
