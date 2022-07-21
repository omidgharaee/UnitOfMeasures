using UnitOfMeasures.Core.Domain.Dimension.Commands;

namespace UnitOfMeasures.Core.Domain.Dimension.Data
{
    public interface IUnitRepository
    {
        bool Exists(Guid id);

        UnitOfMeasures.Core.Domain.Unit.Entities.Unit Load(Guid id);

        void Add(UnitOfMeasures.Core.Domain.Unit.Entities.Unit entity);
        void Remove(UnitOfMeasures.Core.Domain.Unit.Entities.Unit entity);
    }
}
