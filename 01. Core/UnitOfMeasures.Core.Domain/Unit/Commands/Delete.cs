using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitOfMeasures.Core.Domain.Unit.Commands
{
    public class Delete
    {
        public Delete(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
