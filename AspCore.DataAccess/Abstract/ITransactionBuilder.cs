using System;
using System.Collections.Generic;
using System.Text;
using AspCore.Dependency.Abstract;

namespace AspCore.DataAccess.Abstract
{
    public interface ITransactionBuilder
    {
        void BeginTransaction();

        void CommitTransaction();

        void RollbackTransaction();

        void DisposeTransaction();
    }
}
