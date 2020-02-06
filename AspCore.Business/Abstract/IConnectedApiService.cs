using System;
using System.Collections.Generic;
using System.Text;
using AspCore.ApiClient.Abstract;
using AspCore.Dependency.Abstract;

namespace AspCore.Business.Abstract
{
    public interface IConnectedApiService : ITransientType
    {
        /// <summary>
        /// Girilen apiKey ile startup dosyasında api client eklenmesi gerekmektedir
        /// </summary>
        string apiKey { get; }
    }
}
