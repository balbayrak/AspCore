using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspCore.Utilities.DataProtector
{
    public class DataProtectorFactory
    {
        private static DataProtectorFactory _resolver;

        public static DataProtectorFactory Instance
        {
            get
            {
                if (_resolver == null)
                    throw new Exception("DataProtectorFactory not initialized. You should initialize it in Startup class");
                return _resolver;
            }
        }

        private DataProtectorFactory(IDataProtectorHelper dataProtectorHelper)
        {
            _dataProtectorHelper = dataProtectorHelper;
        }
        public static void Init(IDataProtectorHelper dataProtectorHelper)
        {
            if (_resolver == null)
                _resolver = new DataProtectorFactory(dataProtectorHelper);
        }

        private readonly IDataProtectorHelper _dataProtectorHelper;


        public string Protect(string input)
        {
            return _dataProtectorHelper.Protect(input);
        }

        public string UnProtect(string input)
        {
            return _dataProtectorHelper.UnProtect(input);
        }
    }
}
