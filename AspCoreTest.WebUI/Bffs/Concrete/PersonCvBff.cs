using AspCore.BackendForFrontend.Concrete;
using AspCore.Entities.DocumentType;
using AspCoreTest.Bffs.Abstract;
using AspCoreTest.Entities.Models;
using System;
using AspCoreTest.Dtos.Dtos;

namespace AspCoreTest.Bffs.Concrete
{
    public class PersonCvBff : BaseDocumentEntityBffLayer<PersonCvDto, PersonCv, Document>, IPersonCVBff
    {
        public PersonCvBff(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
