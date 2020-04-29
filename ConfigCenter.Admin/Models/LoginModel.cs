using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConfigCenter.Admin.Models
{
    public class LoginModel
    {
        public string name { get; set; }
        public string password { get; set; }
        public string rolename { get; set; }
        public string Salt { get; set; }
    }
}
