using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Utilities.MimeMapping
{
    public interface IMimeMappingService
    {
        string Map(string fileName);
    }
}
