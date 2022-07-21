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
    public class UpdateHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUnitRepository _unitRepository;

        public UpdateHandler(IUnitOfWork unitOfWork, IUnitRepository unitRepository)
        {
            _unitOfWork = unitOfWork;
            _unitRepository = unitRepository;
        }
        public void Handle(Core.Domain.Unit.Commands.Update command)
        {
            _unitOfWork.Commit();
        }
    }
}
