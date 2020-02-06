using AspCore.Business.Abstract;

namespace AspCore.WebApi
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
