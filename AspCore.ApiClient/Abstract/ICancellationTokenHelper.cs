using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using AspCore.Dependency.Abstract;

namespace AspCore.ApiClient.Abstract
{
    public interface ICancellationTokenHelper:ITransientType
    {
        CancellationToken Token { get; }
    }
}
