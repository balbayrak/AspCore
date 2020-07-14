﻿using AspCore.Dependency.DependencyAttributes;
using AspCore.Web.Concrete;
using AspCoreTest.Bffs.Abstract;
using AspCoreTest.Entities.Models;
using AspCoreTest.WebUI.Models.ViewModels;
using System;

namespace AspCoreTest.Bffs.Concrete
{
    public class PersonBff2 : BaseDatatableEntityBffLayer<PersonViewModel, Person>, IPersonBff
    {
        public PersonBff2(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
