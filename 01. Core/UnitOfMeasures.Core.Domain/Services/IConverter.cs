using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitOfMeasures.Core.Domain.Services
{
    public interface IConverter
    {
        string Convert(string convertValue, Core.Domain.Unit.Entities.Unit convertUnit, Core.Domain.Unit.Entities.Unit intoUnit, List<Domain.Unit.Entities.Unit> units);
    }
}
