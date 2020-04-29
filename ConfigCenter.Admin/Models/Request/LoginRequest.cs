using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConfigCenter.Admin.Models.Request
{
    public class LoginRequest
    {
        public string name { get; set; }
        public string password { get; set; }
    }
}
