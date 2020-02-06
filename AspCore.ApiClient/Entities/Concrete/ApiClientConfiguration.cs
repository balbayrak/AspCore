using AspCore.ApiClient.Entities.Abstract;

namespace AspCore.ApiClient.Entities
{
    public class ApiClientConfiguration : IApiClientConfiguration
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
