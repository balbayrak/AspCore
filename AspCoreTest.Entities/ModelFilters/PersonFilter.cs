using AspCore.Entities.EntityFilter;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCoreTest.Entities.ModelFilters
{
    public class PersonFilter : EntityFilter
    {
        public Guid kullaniciUn { get; set; }

        public string name { get; set; }
    }
}
