using AspCore.Business.Abstract;
using AspCore.WebApi;

namespace AspCore.BusinessApi
{
    public abstract class BaseBusinessController<TService> : BaseController
        where TService : IBusinessService
    {
        protected TService _service;

        public BaseBusinessController()
        {
        }
        public BaseBusinessController(TService service)
        {
            _service = service;
        }
    }
}
