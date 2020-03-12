using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Entities.General
{
    public class BaseServiceResult : MarshalByRefObject, IDisposable
    {
        private bool _DisposedValue = false;
        private int _statusCode;
        private string _ErrorMessage;
        private string _WarningMessage;
        private string _ExMessage;
        private string _StatusMessage;

        public BaseServiceResult()
        {
            _statusCode = 400;
            _ErrorMessage = string.Empty;
            _WarningMessage = string.Empty;
            _ExMessage = string.Empty;
            _StatusMessage = string.Empty;
        }
        public int StatusCode
        {
            get { return _statusCode; }
            set { _statusCode = value; }
        }
        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }

        public string StatusMessage
        {
            get { return _StatusMessage; }
            set { _StatusMessage = value; }
        }
        public string WarningMessage
        {
            get { return _WarningMessage; }
            set { _WarningMessage = value; }
        }
        public string ExceptionMessage
        {
            get { return _ExMessage; }
            set { _ExMessage = value; }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this._DisposedValue) return;
            if (disposing)
            {
                this._ExMessage = null;
                this._ErrorMessage = null;
                this._WarningMessage = null;
            }
            this._DisposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
