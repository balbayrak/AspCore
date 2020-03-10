using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Entities.General
{
    public class AjaxResult
    {
        public AjaxResult()
        {
            this.Result = 0;
            this.ResultText = null;
        }
        public AjaxResultTypeEnum Result { get; set; }
        public string ResultText { get;  set; }
    }

    public enum AjaxResultTypeEnum
    {
        Succeed = 1,
        Warning = 2,
        Error = 3
    }
}
