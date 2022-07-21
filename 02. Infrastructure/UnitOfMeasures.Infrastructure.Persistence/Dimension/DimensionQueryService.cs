using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitOfMeasures.Core.Domain.Dimension.Data;
using UnitOfMeasures.Infrastructure.Persistence.Contexts;

namespace UnitOfMeasures.Infrastructure.Persistence.Dimension
{
    public class DimensionQueryService : IDimensionQueryService
    {
        private readonly SQLiteDataBaseContext _dbContext;

        public DimensionQueryService(SQLiteDataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<Core.Domain.Dimension.Entities.Dimension> QueryGetAll()
        {
            return _dbContext.Dimensions.ToList();
        }

        public Core.Domain.Dimension.Entities.Dimension QueryGetWithUnits(Guid id)
        {
            return _dbContext.Dimensions.AsQueryable().Where(d => d.Id == id).Include(d => d.Units).SingleOrDefault();
        }
    }
}
