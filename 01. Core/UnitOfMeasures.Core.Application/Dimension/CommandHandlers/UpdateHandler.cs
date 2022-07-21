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
    public class UpdateHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void Handle()
        {
            _unitOfWork.Commit();
        }
    }
}
