using AspCore.Business.Task.Abstract;
using AspCore.DataAccess.Abstract;
using AspCore.Mapper.Abstract;
using Microsoft.Extensions.DependencyInjection;
using System;
using AspCore.Business.Abstract;

namespace AspCore.Business.Concrete
{
    public abstract class BaseBusinessManager:IBusinessService
    {
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

        protected IServiceProvider ServiceProvider { get; }
        protected ITransactionBuilder TransactionBuilder => LazyGetRequiredService(ref _transactionBuilder);
        private ITransactionBuilder _transactionBuilder;

        private IAutoObjectMapper _autoObjectMapper;
        protected IAutoObjectMapper AutoObjectMapper => LazyGetRequiredService(ref _autoObjectMapper);

        private ITaskBuilder _taskBuilder;
        protected ITaskBuilder TaskBuilder => LazyGetRequiredService(ref _taskBuilder);
        private ITaskFlowBuilder _taskFlowBuilder;
        protected ITaskFlowBuilder TaskFlowBuilder => LazyGetRequiredService(ref _taskFlowBuilder);

        protected BaseBusinessManager(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }


    }
}
