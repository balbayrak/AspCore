using System.Collections.Generic;
using System.Linq;

namespace AspCore.Business.Task.Abstract
{
    public class ValidationItem
    {
        public ITaskValidator _taskValidator { get; set; }

        public List<string> _operations { get; set; }


        public ValidationItem(ITaskValidator taskValidator, params string[] operations)
        {
            _taskValidator = taskValidator;
            _operations = operations != null ? operations.ToList() : null;
        }
    }
}
