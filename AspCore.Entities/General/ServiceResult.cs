using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Entities.General
{
    public class ServiceResult<T> : BaseServiceResult
    {
        private bool _DisposedValue = false;

        private T _Result;

        private int _TotalResultCount;
        private int _SearchResultCount;



        public T Result
        {
            get { return _Result; }
            set
            {
                _Result = value;
            }
        }

        public int TotalResultCount
        {
            get { return _TotalResultCount; }
            set { _TotalResultCount = value; }
        }

        public int SearchResultCount
        {
            get { return _SearchResultCount; }
            set { _SearchResultCount = value; }
        }

        public bool IsSucceededAndDataIncluded()
        {
            if (this.IsSucceeded == true && this.Result != null)
            {
                return true;
            }

            return false;

        }

        public ServiceResult()
        {
            ExceptionMessage = null;
            ErrorMessage = null;
            WarningMessage = null;
            TotalResultCount = 0;
        }

        protected override void Dispose(bool disposing)
        {
            if (this._DisposedValue) return;
            if (disposing)
            {
                base.Dispose();
            }
            this._DisposedValue = true;
        }

    }


}
