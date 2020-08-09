using AspCore.DataAccess.Abstract;

namespace AspCore.DataAccess.EntityFramework
{
    public class EfTransactionBuilder<TDbContext> : ITransactionBuilder
        where TDbContext : CoreDbContext
    {
        private TDbContext _dbContext { get; }
        public EfTransactionBuilder(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void BeginTransaction()
        {
            _dbContext.Database.BeginTransaction();
        }
        public void CommitTransaction()
        {
            _dbContext.Database.CommitTransaction();
        }
        public void RollbackTransaction()
        {
            _dbContext.Database.RollbackTransaction();
        }
        public void DisposeTransaction()
        {
            try
            {
                if (_dbContext.Database.CurrentTransaction != null)
                    _dbContext.Database.CurrentTransaction.Dispose();
            }
            catch
            {

            }
        }

        public void Dispose()
        {
            DisposeTransaction();
        }
    }
}
