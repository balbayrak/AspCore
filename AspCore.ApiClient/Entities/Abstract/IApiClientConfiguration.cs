﻿using System;
using System.Collections.Generic;
using System.Text;
using AspCore.Entities.Configuration;

namespace AspCore.ApiClient.Entities.Abstract
{
    public interface IApiClientConfiguration : IConfigurationEntity
    {
        public string BaseAddress { get; set; }
        public ApiAuthentication Authentication { get; set; }

        /// <summary>
        /// minutes
        /// </summary>
        public long? storageExpireTime { get; set; }

        public string storageKey { get; set; }
    }
}
