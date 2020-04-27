using System;

namespace AspCore.WebComponents.HtmlHelpers.ConfirmBuilder
{
    public class ConfirmManagerFactory
    {
        private static ConfirmManagerFactory _resolver;

        public static ConfirmManagerFactory Instance
        {
            get
            {
                if (_resolver == null)
                    throw new Exception("ConfirmManagerFactory not initialized. You should initialize it in Startup class");
                return _resolver;
            }
        }

        private ConfirmManagerFactory(IConfirmService confirmService)
        {
            _confirmService = confirmService;
        }
    
        public static void Init(IConfirmService confirmService)
        {
            if (_resolver == null)
                _resolver = new ConfirmManagerFactory(confirmService);
        }

        private readonly IConfirmService _confirmService;

        public string GetConfirmString(ConfirmOption confirmOption)
        {
            return _confirmService.GetConfirmString(confirmOption);
        }

    }
}
