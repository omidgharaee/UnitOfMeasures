using Framework.Domain.Entities;
using Framework.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitOfMeasures.Core.Domain.Unit.Entities
{
    public class Unit : BaseEntity<Guid>
    {
      

        public Unit()
        {

        }

        public Unit(Guid id)
        {
            Id = id;


        }

        public string Name { get; set; }
        public string PersianName { get; set; }
        public string Abbreviation { get; set; }
        public double ConversionFactor { get; set; }
        public string FormulaToBase { get; set; }
        public string FormulaFromBase { get; set; }
        public string Type { get; set; }
        public Guid DimensionId { get; set; }
        public Core.Domain.Dimension.Entities.Dimension Dimension { get; set; }

        protected override void ValidateInvariants()
        {
            throw new NotImplementedException();
        }
    }
}
