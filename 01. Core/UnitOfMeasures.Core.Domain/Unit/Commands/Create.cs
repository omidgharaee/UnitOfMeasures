using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitOfMeasures.Core.Domain.Unit.Commands
{
    public class Create
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PersianName { get; set; }
        public string Abbreviation { get; set; }
        public double ConversionFactor { get; set; }
        public string FormulaToBase { get; set; }
        public string FormulaFromBase { get; set; }
        public string Type { get; set; }
        public Guid DimensionId { get; set; }
    }
}
