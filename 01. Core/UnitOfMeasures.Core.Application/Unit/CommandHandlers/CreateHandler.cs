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
    public class CreateHandler : ICommandHandler<Core.Domain.Unit.Commands.Create>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUnitRepository _unitRepository;

        public CreateHandler(IUnitOfWork unitOfWork, IUnitRepository unitRepository)
        {
            _unitOfWork = unitOfWork;
            _unitRepository = unitRepository;
        }
        public void Handle(Core.Domain.Unit.Commands.Create command)
        {
            if (_unitRepository.Exists(command.Id))
                throw new InvalidOperationException($"قبلا با شناسه {command.Id} ثبت شده است.");

            Core.Domain.Unit.Entities.Unit unit = new Core.Domain.Unit.Entities.Unit(command.Id);
            unit.Name = command.Name;
            unit.PersianName = command.PersianName;
            unit.Abbreviation = command.Abbreviation;
            unit.ConversionFactor = command.ConversionFactor;
            unit.FormulaFromBase = command.FormulaFromBase;
            unit.FormulaToBase = command.FormulaToBase;
            unit.Type = command.Type;
            unit.DimensionId = command.DimensionId;

            _unitRepository.Add(unit);
            _unitOfWork.Commit();
        }
    }
}
