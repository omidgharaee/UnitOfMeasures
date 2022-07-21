using Framework.Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitOfMeasures.Infrastructure.Persistence.Contexts;

namespace UnitOfMeasures.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SQLiteDataBaseContext _dbContext;

        public UnitOfWork(Contexts.SQLiteDataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public int Commit()
        {
            int result = _dbContext.SaveChanges();
            return result;
        }
    }
}
