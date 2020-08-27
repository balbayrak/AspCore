using System.Collections.Generic;
using System.Threading.Tasks;
using AspCore.BackendForFrontend.Abstract;
using AspCore.Entities.DocumentType;
using AspCore.Entities.General;
using AspCoreTest.Dtos.Dtos;
using AspCoreTest.Entities.Models;

namespace AspCoreTest.Bffs.Abstract
{
    public interface IPersonCVBff : IDocumentEntityBffLayer<PersonCvDto, PersonCv, Document>
    {
        Task<ServiceResult<List<PersonCvDto>>> GetWithInclude();

    }
}
