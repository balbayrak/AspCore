﻿using AspCoreTest.Entities.Models;
using AspCore.DataAccess.Abstract;

namespace AspCoreTest.DataAccess.Abstract
{
    public interface ICountryDAL : IEntityRepository<Country>
    {
    }
}
