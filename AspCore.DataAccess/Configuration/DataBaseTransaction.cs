using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AspCore.DataAccess.Configuration
{
    public class DataBaseTransaction
    {
        public IsolationLevel isolationLevel { get; set; }

        public DataBaseTransaction()
        {
            isolationLevel = IsolationLevel.ReadUncommitted;
        }
    }
}
