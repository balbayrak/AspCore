using AspCore.BackendForFrontend.Abstract;
using AspCore.BackendForFrontend.Concrete.Security.User;
using AspCore.Storage.Abstract;
using AspCore.Storage.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspCore.BackendForFrontend.Concrete
{
    public abstract class BaseBffLayer : IBffLayer
    {
        protected readonly ICurrentUser CurrentUser;
       
        public IBffApiClient ApiClient { get; private set; }

        private string _apiClientKey { get; set; }
        protected string apiClientKey
        {
            get
            {
                return _apiClientKey;
            }
            set
            {
                _apiClientKey = value;
                ApiClient.ChangeApiSettingsKey(value);
            }
        }

        private string _apiControllerRoute;
        protected string apiControllerRoute
        {
            get { return _apiControllerRoute; }
            set
            {
                if (!value.StartsWith("/"))
                {
                    value = "/" + value;
                }
                _apiControllerRoute = value;
            }
        }

        protected IServiceProvider ServiceProvider { get; private set; }
        protected readonly object ServiceProviderLock = new object();

        protected TService LazyGetRequiredService<TService>(ref TService reference)
    => LazyGetRequiredService(typeof(TService), ref reference);

        protected TRef LazyGetRequiredService<TRef>(Type serviceType, ref TRef reference)
        {
            if (reference == null)
            {
                lock (ServiceProviderLock)
                {
                    if (reference == null)
                    {
                        reference = (TRef)ServiceProvider.GetRequiredService(serviceType);
                    }
                }
            }

            return reference;
        }

        protected StorageService StorageManager => LazyGetRequiredService(ref _storageService);
        private StorageService _storageService;

        protected BaseBffLayer(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;

            ApiClient = ServiceProvider.GetRequiredService<IBffApiClient>();


            CurrentUser = ServiceProvider.GetRequiredService<ICurrentUser>();

        }

    }
}
