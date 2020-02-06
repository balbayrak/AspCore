using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Entities.User
{
    public class ActiveUserFilter
    {
        public string ticketStr { get; set; }
        public byte[] ticket { get; set; }

    }
}
