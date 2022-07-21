using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitOfMeasures.Core.Domain.Services
{
    public interface IMathParser
    {
        double Parse(string expression);
    }
}
