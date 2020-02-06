using System;
using System.Collections.Generic;
using System.Text;
using AspCore.Business.Task.Abstract;

namespace AspCore.Business.Task.Concrete
{
    internal class TaskFlowItem
    {
        public ITask _task { get; set; }
        public int _order { get; set; }

        public TaskFlowItem(ITask task,int order)
        {
            this._task = task;
            this._order = order;
        }
    }
}
