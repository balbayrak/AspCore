using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using AspCore.Entities.Constants;
using AspCore.Extension;
using Microsoft.AspNetCore.Http;

namespace AspCore.ApiClient
{
    public class CoreHttpClient : HttpClient
    {
        public static TimeSpan DefaultTimeout { get; set; } = TimeSpan.FromMinutes(2);
        public CoreHttpClient(HttpMessageHandler handler, TimeSpan? timeOut = null) : base(handler)
        {
            Timeout = timeOut ?? DefaultTimeout;
            DefaultRequestHeaders.Accept.Clear();
            DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ApiConstants.Api_Keys.JSON_MEDIA_TYPE_QUALITY_HEADER));
            DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue(ApiConstants.Api_Keys.GZIP_COMPRESSION_STRING_WITH_QUALITY_HEADER));
        }

        public CoreHttpClient(TimeSpan? timeOut = null)
        {
            Timeout = timeOut ?? DefaultTimeout;
            DefaultRequestHeaders.Accept.Clear();
            DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ApiConstants.Api_Keys.JSON_MEDIA_TYPE_QUALITY_HEADER));
            DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue(ApiConstants.Api_Keys.GZIP_COMPRESSION_STRING_WITH_QUALITY_HEADER));

        }
    }
}
