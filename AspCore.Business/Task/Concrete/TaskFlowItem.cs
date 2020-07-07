using System;
using System.Collections.Generic;
using System.Text;
using AspCore.Business.Task.Abstract;

namespace AspCore.Business.Task.Concrete
{
    public class TaskFlowItem
    {
        public ITask task { get; set; }
        public int order { get; set; }

        public TaskFlowItem(ITask task,int order)
        {
            this.task = task;
            this.order = order;
        }
    }
}
