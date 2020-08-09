﻿using AspCore.ApiClient.Handlers;
using System;

namespace AspCore.ApiClient.Configuration
{
    public class ApiClientOption
    {
        /// <summary>
        /// configuration key
        /// </summary>
        public string apiKey { get; set; }
        /// <summary>
        /// HttpClient timeout value(minutes)
        /// </summary>
        public int timeout { get; set; } = 2;
        /// <summary>
        /// Retry Count for unsuccessful request
        /// </summary>
        public int retryCount { get; set; } = 3;
        /// <summary>
        /// Cicuit after unsuccessful request count
        /// </summary>
        public int circuitbreakerCount { get; set; } = 5;

    }
}
