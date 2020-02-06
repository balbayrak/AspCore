using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.AOP.Abstract
{
    public abstract class InterceptorBase : Attribute, IInterceptor, IDisposable
    {
        public int priority { get; set; }
        protected IInterceptorContext _context { get; set; }
        public InterceptorBase()
        {
        }
        public InterceptorBase(IInterceptorContext context)
        {
            _context = context;
        }


        public void Dispose()
        {
            this._context = null;
        }
    }
}
