using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Entities.Authentication
{
    public class AuthorizationFilterOption
    {

        /// <summary>
        /// only include controller fire for global filter, another controllers skips
        /// </summary>
        public string[] includeControllerNames { get; set; }

        /// <summary>
        /// exclude controller for global filter
        /// </summary>
        public string[] excludeControllerNames { get; set; }
    }
}
