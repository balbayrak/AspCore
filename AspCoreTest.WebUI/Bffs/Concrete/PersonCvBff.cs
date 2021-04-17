using AspCore.BackendForFrontend.Concrete;
using AspCore.Entities.DocumentType;
using AspCoreTest.Bffs.Abstract;
using AspCoreTest.Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspCore.Entities.Constants;
using AspCore.Entities.General;
using AspCoreTest.Dtos.Dtos;

namespace AspCoreTest.Bffs.Concrete
{
    public class PersonCvBff : BaseDocumentEntityBffLayer< PersonCv, Document, PersonCvDto>, IPersonCVBff
    {
        public PersonCvBff(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<ServiceResult<List<PersonCvDto>>> GetWithInclude()
        {
            ApiClient.apiUrl = apiControllerRoute + "/GetWithInclude";
            var result = await ApiClient.PostRequest<ServiceResult<List<PersonCvDto>>>(string.Empty);
            return result;
        }
    }
}
