using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspCore.ApiClient.Entities.Concrete
{
    public class AuthenticationTokenResponse
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public DateTime expires { get; set; }

    }
}
