﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using AspCore.ApiClient.Abstract;
using Microsoft.AspNetCore.Http;

namespace AspCore.ApiClient
{
    public class HttpContextCancellationTokenHelper:ICancellationTokenHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CancellationToken Token => _httpContextAccessor.HttpContext?.RequestAborted ?? CancellationToken.None;

        public HttpContextCancellationTokenHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}