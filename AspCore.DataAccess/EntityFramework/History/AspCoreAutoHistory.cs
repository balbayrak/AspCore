using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.DataAccess.EntityFramework.History
{
    public class AspCoreAutoHistory : AutoHistory
    {
        public Guid ActiveUserID { get; set; }
    }
}
