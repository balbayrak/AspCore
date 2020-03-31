using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspCore.CacheApi;
using AspCore.CacheEntityApi.CacheProviders.Abstract;
using AspCoreTest.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspCoreTest.CacheApi.Controllers
{
    public class PersonCacheController : BaseCacheEntityController<ICacheEntityProvider<Person>,Person>
    {
    }
}