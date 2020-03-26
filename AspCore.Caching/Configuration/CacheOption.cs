﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AspCore.Caching.Abstract;
using AspCore.Caching.Concrete;

namespace AspCore.Caching.Configuration
{
    public class CacheOption
    {
        public EnumCache cacheType { get; set; }
    }
}
