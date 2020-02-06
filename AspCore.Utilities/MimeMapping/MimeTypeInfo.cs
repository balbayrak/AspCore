using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Utilities.MimeMapping
{
    public class MimeTypeInfo
    {
        /// <summary>
        /// exp:.pdf,.123
        /// </summary>
        public string extension { get; set; }

        /// <summary>
        /// exp:application/pdf,aplication/123
        /// </summary>
        public string mimetype { get; set; }
    }
}
