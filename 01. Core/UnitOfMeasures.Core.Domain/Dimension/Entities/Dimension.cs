using Framework.Domain.Entities;
using Framework.Domain.Events;
using Framework.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitOfMeasures.Core.Domain.Dimension.Entities
{
    public class Dimension : BaseEntity<Guid>
    {
        public Dimension()
        {

        }
        public Dimension(Guid id, string name)
        {
            Id = id;
            Name = name;
            ValidateInvariants();
        }

        public string Name { get; set; }
        public List<Unit.Entities.Unit> Units { get; set; }

        protected override void ValidateInvariants()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new InvalidEntityStateException(this, $"Name Can Not Is Null Or WhiteSpace!");
            }
        }
    }
}
