using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitOfMeasures.Core.Domain.Dimension.Commands;
using UnitOfMeasures.Core.Domain.Dimension.Data;
using UnitOfMeasures.Infrastructure.Persistence.Contexts;

namespace UnitOfMeasures.Infrastructure.Persistence.Unit
{
    public class EfUnitRepository : IUnitRepository, IDisposable
    {
        private readonly SQLiteDataBaseContext _dbContext;

        public EfUnitRepository(SQLiteDataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Core.Domain.Unit.Entities.Unit entity)
        {
            _dbContext.Add(entity);
        } 

        public void Remove(Core.Domain.Unit.Entities.Unit entity)
        {
            _dbContext.Remove(entity);
        }

        public bool Exists(Guid id)
        {
            return _dbContext.Dimensions.Any(c => c.Id == id);
        }

        public Core.Domain.Unit.Entities.Unit Load(Guid id)
        {
            return _dbContext.Units.Find(id);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

    }
}
