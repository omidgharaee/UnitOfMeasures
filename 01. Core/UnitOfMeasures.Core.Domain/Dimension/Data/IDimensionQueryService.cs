using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitOfMeasures.Core.Domain.Dimension.Data
{
    public interface IDimensionQueryService
    {
        List<Core.Domain.Dimension.Entities.Dimension> QueryGetAll(); 
        Core.Domain.Dimension.Entities.Dimension QueryGetWithUnits(Guid id);
    }
}
