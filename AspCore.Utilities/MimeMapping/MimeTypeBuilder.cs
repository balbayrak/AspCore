using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using AspCore.Entities.Configuration;

namespace AspCore.Utilities.MimeMapping
{
    public class MimeTypeBuilder : ConfigurationOption
    {
        private List<MimeTypeInfo> mimeTypes { get; set; }

        public MimeTypeBuilder(IServiceCollection services) : base(services)
        {
        }
        /// <summary>
        /// This method add custom mimeType to FileExtensionContentTypeProvider
        /// </summary>
        /// <param name="extension">exp:.pdf,.123</param>
        /// <param name="mimeType">exp:application/pdf,aplication/123</param>
        /// <returns></returns>
        public MimeTypeBuilder AddMimeType(string extension, string mimeType)
        {
            mimeTypes = mimeTypes ?? new List<MimeTypeInfo>();
            mimeTypes.Add(new MimeTypeInfo
            {
                extension = extension,
                mimetype = mimeType
            });

            return this;
        }

        /// <summary>
        /// This method runs always, if not add custom mimeType
        /// </summary>
        public void Build()
        {
            var provider = new FileExtensionContentTypeProvider();

            if (mimeTypes != null && mimeTypes.Count > 0)
            {
                foreach (var item in mimeTypes)
                {
                    provider.Mappings.Add(item.extension, item.mimetype);
                }
            }

            services.AddSingleton<IMimeMappingService>(new MimeMappingManager(provider));
        }
    }
}
