using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;
using AspCore.Entities.Json;

namespace AspCore.Entities.User
{
    public class ActiveUser : IActiveUser
    {
        public Guid id { get; set; }
        public string tckn { get; set; }
        public string username { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public string job { get; set; }
        public string jobCompany { get; set; }
        public string address { get; set; }
        public string telephone { get; set; }
        public List<string> roles { get; set; }
        public string correlationId { get; set; }
        public Guid activeUserId { get; set; }

        public ActiveUser()
        {

        }

        public ActiveUser(string json)
        {
            ActiveUser info = null;
            if (json != null)
            {
                json = HttpUtility.UrlDecode(json);

                info = JsonConvert.DeserializeObject<ActiveUser>(json, new DecimalJsonConverter());

                this.id = info.id;
                this.tckn = info.tckn;
                this.username = info.username;
                this.name = info.name;
                this.email = info.email;
                this.job = info.job;
                this.jobCompany = info.jobCompany;
                this.telephone = info.telephone;
                this.roles = info.roles;
                this.correlationId = info.correlationId;
            }
        }
    }
}
