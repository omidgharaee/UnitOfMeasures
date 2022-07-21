using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Domain.ApplicationServices
{
    public interface ICommandHandler<TCommand>
    {
        void Handle(TCommand command);
    }
}
