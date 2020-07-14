using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Utilities.DataProtector
{
    public class DataProtectorOption
    {
        public string dataProtectorKey { get; set; }

        /// <summary>
        /// as day
        /// </summary>
        public int lifeTime { get; set; } = 90;

        public string persistFileSytemPath { get; set; }

    }
}
