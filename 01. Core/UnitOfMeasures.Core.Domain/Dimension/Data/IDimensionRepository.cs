using UnitOfMeasures.Core.Domain.Dimension.Commands;

namespace UnitOfMeasures.Core.Domain.Dimension.Data
{
    public interface IDimensionRepository
    {
        bool Exists(Guid id);

        UnitOfMeasures.Core.Domain.Dimension.Entities.Dimension Load(Guid id);

        void Add(UnitOfMeasures.Core.Domain.Dimension.Entities.Dimension entity);
        void Remove(Core.Domain.Dimension.Entities.Dimension entity);
    }
}
